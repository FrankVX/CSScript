using Stone.AST;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stone
{
    public class BaseParser
    {
        Dictionary<string, int> ops = new Dictionary<string, int>() {
            { "=",-1},
            { "+=",-1},
            { "-=",-1},
            { "==",0},
            { ">",0},
            { "<",0},
            { "+",1},
            { "-",1},
            { "*",2},
            { "/",2},
            { "%",2},
        };

        Parser
            primary = Parser.rule<PrimaryTree>(),
            block = Parser.rule("block"),
            simple = Parser.rule("simple"),
            statement = Parser.rule("statement"),
            exp = Parser.rule<BinaryTree>(),
            whileStmnt = Parser.rule<WhileTree>(),
            ifStmnt = Parser.rule<IfTree>(),
            program = Parser.rule("program");


        public BaseParser()
        {
            exp.exp(primary, ops);
            primary.or(
                Parser.rule("(exp)").str("(").ast(exp).str(")"),
                Parser.rule("token").token(Token.TokenType.Identifier, Token.TokenType.Number, Token.TokenType.String)
                );
            simple.ast(exp).option(Parser.rule(";").str(";"));
            block.str("{").repet(statement).str("}");
            statement.or(ifStmnt, whileStmnt, block, simple);
            ifStmnt.str("if").ast(primary).ast(statement).option(Parser.rule("option else").str("else").ast(statement));
            whileStmnt.str("while").ast(primary).ast(statement);
            program.option(Parser.rule("repet statement").repet(statement)).token(Token.TokenType.EOF);

            var postfix = Parser.rule("postfix").str("(").repet(exp).str(")");
            primary.option(postfix);
        }

        internal virtual ASTNode Parse(Lexer lexer)
        {
            return program.Parse(lexer);
        }
    }
}
