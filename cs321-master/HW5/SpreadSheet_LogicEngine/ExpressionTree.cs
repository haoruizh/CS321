namespace CS321
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using SpreadSheet_LogicEngine;

    /// <summary>
    /// expression tree class.
    /// </summary>
    public class ExpressionTree
    {
        private BinaryOperatorNode root;
        private Dictionary<string, double> keyValuePairs = new Dictionary<string,double>();
        private string expression;

        /// <summary>
        /// Find the operator from expression.
        /// </summary>
        /// <param name="expression">Input expression</param>
        /// <returns>return the first operator in the expression.</returns>
        private char FindOperator(string expression)
        {
            char op = expression[0];
            for (int i = expression.Length - 1; i >= 0; --i)
            {
                char c = expression[i];
                if (this.IsOperator(c))
                {
                    op = c;
                    break;
                }
            }
            return op;
        }

        /// <summary>
        /// check if the input string contain any valid operator.
        /// </summary>
        /// <param name="expression">Passed in expression</param>
        /// <returns>True if it expression contains op, else False.</returns>
        private bool ContainOp(string expression)
        {
            if (expression.Contains('+') | expression.Contains('-') | expression.Contains('*') | expression.Contains('/') | expression.Contains('^'))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// check if input c is a valid operator for this tree.
        /// </summary>
        /// <param name="c">input character.</param>
        /// <returns>True if c is op, else false.</returns>
        private bool IsOperator(char c)
        {
            if (c == '+' || c == '-' || c == '*'
            || c == '/')
            {
                return true;
            }

            return false;
        }

        private double Evaluate(BaseNode node)
        {
            double result = 0;
            if (node == null)
            {
                return 0;
            }

            // as a constant node
            ConstantNumbericalNode constantNode = node as ConstantNumbericalNode;
            if (constantNode != null)
            {
                result = constantNode.Value;
                return result;
            }

            // as a variable
            VariableNode variableNode = node as VariableNode;
            if (variableNode != null)
            {
                result = this.keyValuePairs[variableNode.Name];
                return result;
            }

            // an operator node
            BinaryOperatorNode operatorNode = node as BinaryOperatorNode;
            if (operatorNode != null)
            {
                // choose operator
                switch (operatorNode.BinaryOperator)
                {
                    case '+':
                        result = this.Evaluate(operatorNode.Left) + this.Evaluate(operatorNode.Right);
                        break;
                    case '-':
                        result = this.Evaluate(operatorNode.Left) - this.Evaluate(operatorNode.Right);
                        break;
                    case '*':
                        result = this.Evaluate(operatorNode.Left) * this.Evaluate(operatorNode.Right);
                        break;
                    case '/':
                        result = this.Evaluate(operatorNode.Left) / this.Evaluate(operatorNode.Right);
                        break;
                }

                return result;
            }

            return 0;
        }

        /// <summary>
        /// Convert an infix expression to postfix format
        /// Reference: https://www.geeksforgeeks.org/stack-set-2-infix-to-postfix/.
        /// </summary>
        /// <param name="infix">Infix format expression.</param>
        /// <returns>postfix format expression.</returns>
        private string ToPostFix(string infix)
        {
            string result = "";
            if(string.IsNullOrEmpty(infix))
            {
                return result ;
            }
            Stack<char> stack = new Stack<char>();
            //get the length of expression
            int length = infix.Length;
            //reading expression
            for(int i = length-1; i>=0; i--)
            {
                char c = infix[i];
                
                if(char.IsLetterOrDigit(c))
                {
                    //if current c is an operand, add it to the output
                    result += c;
                }
                else if(c=='(')
                {
                    //if the scanned character is an '(', push it to the stack
                    stack.Push(c);
                }
                else if(c==')')
                {
                    //if c is an ')', pop from the stack until reach next ')'
                    while(stack.Count>0 && stack.Peek() != '(')
                    {
                        result += stack.Pop();
                    }
                    if (stack.Count > 0 && stack.Peek() != '(')
                    {
                        return "Invalid Expression";
                    }
                    else
                    {
                        stack.Pop();
                    }
                }
                else
                {
                    //c is an opeartor
                    while (stack.Count > 0)
                    {
                        result += stack.Pop();
                    }
                    stack.Push(c);
                }
            }
            //pop all opeartors from stack
            while (stack.Count>0)
            {
                result += stack.Pop();
            }

            return result;
           
        }

        /// <summary>
        /// construct a new tree based on the new expression
        /// </summary>
        /// <param name="expression">return the root of the new tree</param>
        private BinaryOperatorNode ConstructTree(string expression, char op)
        {
            if (string.IsNullOrEmpty(expression))
            {
                // empty expression.
                return null;
            }

            for (int i = expression.Length - 1; i >= 0; --i)
            {
                char c = expression[i];
                // c is an operator;
                if (c == op)
                {
                    // in this part, dont need to work on precedence
                    BinaryOperatorNode newRoot = new BinaryOperatorNode(c);
                    string leftPart = expression.Substring(0, i);
                    string rightPart = expression.Substring(i + 1);
                    if (!this.ContainOp(leftPart))
                    {
                        // if the left part has no operator:
                        newRoot.Left = this.ConstructTree(leftPart);
                    }
                    else
                    {
                        // if the left part has operator
                        newRoot.Left = this.ConstructTree(leftPart,c );
                    }

                    if (!this.ContainOp(rightPart))
                    {
                        // if the right part has no operator
                        newRoot.Right = this.ConstructTree(rightPart);
                    }
                    else
                    {
                        newRoot.Right = this.ConstructTree(rightPart,c );
                    }

                    return newRoot;
                }
            }

            return null;
        }

        /// <summary>
        /// Non operation node construct.
        /// </summary>
        /// <param name="expression">given expression.</param>
        /// <returns>a BaseNode.</returns>
        private BaseNode ConstructTree(string expression)
        {
            BaseNode newNode;
            if (char.IsDigit(expression[0]))
            {
                // constant numerical node
                newNode = new ConstantNumbericalNode(Convert.ToDouble(expression));
                return newNode;
            }
            else if (char.IsLetter(expression[0]))
            {
                // Variable node
                if (!this.keyValuePairs.ContainsKey(expression))
                {
                    // if dictionary does not contain the expression key
                    this.keyValuePairs.Add(expression, 0);
                }

                // construct the new node
                newNode = new VariableNode(expression);
                return newNode;
            }

            return null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpressionTree"/> class.
        /// Implement this constructor to construct the tree from the specific expression. 
        /// </summary>
        /// <param name="expression">Passedin expression.</param>
        public ExpressionTree(string expression)
        {
            this.expression = expression;
            char op = this.FindOperator(expression);
            this.root = this.ConstructTree(this.expression, op);
        }

        /// <summary>
        /// Gets or sets getter and setter for expression property.
        /// </summary>
        public string Expression
        {
            get
            {
                return this.expression;
            }

            set
            {
                this.expression = value;
                char op = this.FindOperator(this.expression);
                this.root = this.ConstructTree(this.expression, op);
            }
        }

        /// <summary>
        /// Sets the specified variable within the ExpressionTree variables dictionary
        /// </summary>
        /// <param name="variableName">name of variable need to be changed</param>
        /// <param name="variableValue">changing value</param>
        public void SetVariable(string variableName, double variableValue)
        {
            this.keyValuePairs[variableName] = variableValue;
        }

        /// <summary>
        /// Implement this method with no parameters that evaluates the expression to a double
        /// value.
        /// </summary>
        /// <returns>result.</returns>
        public double Evaluate()
        {
            return this.Evaluate(this.root);
        }
    }
}
