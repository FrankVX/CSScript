using System;
using System.Collections.Generic;
using System.Text;

namespace Stone
{
    class Parser
    {
        abstract class Element
        {
            internal abstract bool match(Lexer lexer);
            internal virtual void parse(Lexer lexer, List<ASTNode> nodes)
            {
                if (!match(lexer))
                {
                    throw new Exception("get error ---> " + lexer.Next().value);
                }
            }
        }

        class TokenElement : Element
        {
            HashSet<Token.TokenType> types;
            public TokenElement(params Token.TokenType[] types)
            {
                this.types = new HashSet<Token.TokenType>(types);
            }
            internal override bool match(Lexer lexer)
            {
                if (types.Count == 0) return true;
                return types.Contains(lexer.Peek().type);
            }

            internal override void parse(Lexer lexer, List<ASTNode> nodes)
            {
                base.parse(lexer, nodes);
                nodes.Add(new ASTLeaf(lexer.Next()));
            }
        }

        class StringElement : Element
        {
            string[] strs;
            public StringElement(params string[] strs)
            {
                this.strs = strs;
            }
            internal override bool match(Lexer lexer)
            {
                var t = lexer.Peek();
                foreach (var s in strs)
                    if (t.value == s) return true;
                return false;
            }

            internal override void parse(Lexer lexer, List<ASTNode> nodes)
            {
                base.parse(lexer, nodes);
                lexer.Next();
            }
        }

        class BinaryExpElement : Element
        {
            Dictionary<string, int> ops;
            Parser self, parser;

            public BinaryExpElement(Parser self, Parser parser, Dictionary<string, int> ops)
            {
                this.parser = parser;
                this.ops = ops;
                this.self = self;
            }
            internal override bool match(Lexer lexer)
            {
                return parser.Match(lexer);
            }

            internal override void parse(Lexer lexer, List<ASTNode> nodes)
            {
                base.parse(lexer, nodes);
                var tree = parser.Parse(lexer);
                while (nextIsOp(lexer, out int priority))
                {
                    tree = getExp(lexer, tree, priority);
                }
                nodes.Add(tree);
            }

            internal void Add(string name, int priority)
            {
                ops[name] = priority;
            }

            bool nextIsOp(Lexer lexer, out int priority)
            {
                var op = lexer.Peek().value;
                return ops.TryGetValue(op, out priority);
            }

            ASTNode getExp(Lexer lexer, ASTNode left, int leftP)
            {
                var list = new List<ASTNode>();
                list.Add(left);
                list.Add(new ASTLeaf(lexer.Next()));
                var right = parser.Parse(lexer);
                while (nextIsOp(lexer, out int rightP) && rightP > leftP)
                {
                    right = getExp(lexer, right, rightP);
                }
                list.Add(right);
                return self.result(list);
            }

        }

        class TreeElement : Element
        {
            Parser parser;
            public TreeElement(Parser parser)
            {
                this.parser = parser;
            }
            internal override bool match(Lexer lexer)
            {
                return parser.Match(lexer);
            }

            internal override void parse(Lexer lexer, List<ASTNode> nodes)
            {
                base.parse(lexer, nodes);
                nodes.Add(parser.Parse(lexer));
            }
        }

        class OrTreeElement : Element
        {
            Parser[] parsers;
            public OrTreeElement(params Parser[] parsers)
            {
                this.parsers = parsers;
            }

            internal override bool match(Lexer lexer)
            {
                foreach (var p in parsers)
                {
                    if (p.Match(lexer)) return true;
                }
                return false;
            }

            internal override void parse(Lexer lexer, List<ASTNode> nodes)
            {
                base.parse(lexer, nodes);
                foreach (var p in parsers)
                {
                    if (p.Match(lexer))
                    {
                        nodes.Add(p.Parse(lexer));
                        return;
                    }
                }
            }
        }

        class RepetTreeElement : Element
        {
            Parser parser;
            bool isOnce;
            public RepetTreeElement(Parser parser, bool isOnce)
            {
                this.parser = parser;
                this.isOnce = isOnce;
            }

            internal override bool match(Lexer lexer)
            {
                return parser.Match(lexer);
            }

            internal override void parse(Lexer lexer, List<ASTNode> nodes)
            {
                while (parser.Match(lexer))
                {
                    nodes.Add(parser.Parse(lexer));
                    if (isOnce) break;
                }
            }
        }

        List<Element> childs = new List<Element>();
        internal Type returnType { get; set; } = typeof(ASTTree);
        internal string Name { get; private set; }
        internal ASTNode Parse(Lexer lexer)
        {
            List<ASTNode> nodes = new List<ASTNode>();
            foreach (var e in childs) e.parse(lexer, nodes);
            if (nodes.Count == 1)
            {
                return nodes[0];
            }
            return result(nodes);
        }

        ASTNode result(List<ASTNode> list)
        {
            var tree = Activator.CreateInstance(returnType) as ASTTree;
            tree.SetChilds(list);
            tree.Name = Name;
            return tree;
        }

        public Parser(string name)
        {
            Name = name;
        }

        public Parser(Type type)
        {
            Name = type.Name;
            returnType = type;
        }

        internal bool Match(Lexer lexer)
        {
            if (childs.Count == 0) return true;
            return childs[0].match(lexer);
        }

        internal static Parser rule(string name)
        {
            return new Parser(name);
        }
        internal static Parser rule<T>()
        {
            return new Parser(typeof(T));
        }

        internal Parser token(params Token.TokenType[] types)
        {
            childs.Add(new TokenElement(types));
            return this;
        }

        internal Parser str(params string[] strs)
        {
            childs.Add(new StringElement(strs));
            return this;
        }

        internal Parser exp(Parser parser, Dictionary<string, int> ops)
        {
            childs.Add(new BinaryExpElement(this, parser, ops));
            return this;
        }

        internal Parser ast(Parser ps)
        {
            childs.Add(new TreeElement(ps));
            return this;
        }
        internal Parser or(params Parser[] ps)
        {
            childs.Add(new OrTreeElement(ps));
            return this;
        }
        internal Parser repet(Parser ps)
        {
            childs.Add(new RepetTreeElement(ps, false));
            return this;
        }

        internal Parser option(Parser ps)
        {
            childs.Add(new RepetTreeElement(ps, true));
            return this;
        }
    }
}
