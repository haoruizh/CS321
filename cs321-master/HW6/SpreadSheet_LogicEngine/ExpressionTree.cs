namespace CS321
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using SpreadSheet_LogicEngine;

    /// <summary>
    /// Expression tree class.
    /// </summary>
    public class ExpressionTree
    {
        private BinaryOperatorNode root;
        private Dictionary<string, double> keyValuePairs = new Dictionary<string, double>();
        private string expression;

        /// <summary>
        /// Find the operator from expression.
        /// </summary>
        /// <param name="expression">Input expression.</param>
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
        /// <param name="expression">Input expression.</param>
        /// <returns>bool value to determine if expression has op.</returns>
        private bool ContainOp(string expression)
        {
            return expression.Contains('+') | expression.Contains('-') | expression.Contains('*') | expression.Contains('/') | expression.Contains('^');
        }

        /// <summary>
        /// check if input c is a valid operator for this tree.
        /// </summary>
        /// <param name="c">input character.</param>
        /// <returns></returns>
        private bool IsOperator(char c)
        {
            if (c == '+' || c == '-' || c == '*'
            || c == '/' || c == '^')
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
                result =  constantNode.Value;
                return result;
            }

            // as a variable
            VariableNode variableNode = node as VariableNode;
            if (variableNode != null)
            {
                // if the variable node name is in the dictionary
                if (this.keyValuePairs.ContainsKey(variableNode.Name))
                {
                    result = this.keyValuePairs[variableNode.Name];
                    return result;
                }

                // otherwise return 0
                return 0;
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
        /// Convert an infix expression to postfix format.
        /// </summary>
        /// Reference: https://www.geeksforgeeks.org/stack-set-2-infix-to-postfix/
        /// <param name="infix">Infxi expression.</param>
        /// <returns>Postfix expression queue. </returns>
        private Queue<string> ToPostFix(string infix)
        {
            if (string.IsNullOrEmpty(infix))
            {
                return null;
            }

            Stack<string> tempStack = new Stack<string>();
            Queue<string> result = new Queue<string>();
            int i;
            string currentPart = string.Empty; // store variable name

            // read through the expression
            for (i = 0; i < infix.Length; ++i)
            {
                // current char
                char c = infix[i];

                // if current character is a letter or digit
                if (char.IsLetterOrDigit(c))
                {
                    currentPart += c;
                }

                // read left parenthesis
                else if (c == '(')
                {
                    tempStack.Push(c.ToString());
                }

                // read right parenthesis
                else if (c == ')')
                {
                    result.Enqueue(currentPart);
                    currentPart = string.Empty;
                    while (tempStack.Count > 0 && tempStack.Peek() != "(")
                    {
                        result.Enqueue(tempStack.Pop());
                    }

                    if (tempStack.Count > 0 && tempStack.Peek() != "(")
                    {
                        return null;
                    }
                    else
                    {
                        tempStack.Pop();
                    }
                }

                // an operator is encounted
                else
                {
                    // enqueue the currentpart and make it empty
                    result.Enqueue(currentPart);
                    currentPart = string.Empty;
                    while (tempStack.Count > 0 && this.GetPrecedence(c) <= this.GetPrecedence(Convert.ToChar(tempStack.Peek())))
                    {
                        result.Enqueue(tempStack.Pop());
                    }

                    // push op
                    tempStack.Push(c.ToString());
                }
            }

            // push the last part of expression in to the queue
            result.Enqueue(currentPart);

            // pop all remain in the stack to queue
            while (tempStack.Count > 0)
            {
                result.Enqueue(tempStack.Pop());
            }

            // return result queue
            return result;
        }

        /// <summary>
        /// Return the precedence based on input operator.
        /// </summary>
        /// <param name="c">input char.</param>
        /// <returns>return precedence.</returns>
        private int GetPrecedence(char c)
        {
            if (c == '*' | c == '/')
            {
                return 2;
            }
            else if (c == '+' | c == '-')
            {
                return 1;
            }
            else if (c == '^')
            {
                return 3;
            }

            return -1;
        }

        /// <summary>
        /// construct a new tree based on the new expression.
        /// </summary>
        /// <param name="expression">return the root of the new tree.</param>
        private BinaryOperatorNode ConstructTree(string expression)
        {
            if (string.IsNullOrEmpty(expression))
            {
                // empty expression
                return null;
            }

            // create the tree stack to store nodes
            Stack<BaseNode> treeStack = new Stack<BaseNode>();
            Queue<string> postfixQueue = new Queue<string>();

            // get the postfix expression stored in a queue properly
            postfixQueue = this.ToPostFix(expression);

            // construct the tree
            while (postfixQueue.Count > 0)
            {
                string currentPart = postfixQueue.Dequeue();
                if (!string.IsNullOrEmpty(currentPart))
                {
                    // if next queue element is operand, push it to stack
                    if (char.IsLetterOrDigit(currentPart[0]))
                    {
                        // if currentPart is a number.
                        if (char.IsDigit(currentPart[0]))
                        {
                            ConstantNumbericalNode newNode = new ConstantNumbericalNode(Convert.ToDouble(currentPart));
                            treeStack.Push(newNode);
                        }

                        // if currentPart is a string
                        else if (char.IsLetter(currentPart[0]))
                        {
                            VariableNode newNode = new VariableNode(currentPart);
                            treeStack.Push(newNode);
                        }
                    }

                    // if next queue element is operator
                    else if (this.IsOperator(currentPart[0]))
                    {
                        switch (currentPart[0])
                        {
                            case '+':
                                Addition newAdd = new Addition();
                                newAdd.Right = treeStack.Pop();
                                newAdd.Left = treeStack.Pop();
                                treeStack.Push(newAdd);
                                break;

                            case '-':
                                Minus newMin = new Minus();
                                newMin.Right = treeStack.Pop();
                                newMin.Left = treeStack.Pop();
                                treeStack.Push(newMin);
                                break;
                            case '*':
                                Multiple newMul = new Multiple();
                                newMul.Right = treeStack.Pop();
                                newMul.Left = treeStack.Pop();
                                treeStack.Push(newMul);
                                break;
                            case '/':
                                Divide newDiv = new Divide();
                                newDiv.Right = treeStack.Pop();
                                newDiv.Left = treeStack.Pop();
                                treeStack.Push(newDiv);
                                break;
                        }
                    }
                }
            }

            return (BinaryOperatorNode)treeStack.Pop();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpressionTree"/> class.
        /// Implement this constructor to construct the tree from the specific expression.
        /// </summary>
        /// <param name="expression">Input expression.</param>
        public ExpressionTree(string expression)
        {
            this.expression = expression;
            this.root = this.ConstructTree(this.expression);
        }

        /// <summary>
        /// Gets or sets for expression property.
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
                this.root = this.ConstructTree(this.expression);
            }
        }

        /// <summary>
        /// Sets the specified variable within the ExpressionTree variables dictionary.
        /// </summary>
        /// <param name="variableName">name of variable need to be changed.</param>
        /// <param name="variableValue">changing value.</param>
        public void SetVariable(string variableName, double variableValue)
        {
           this.keyValuePairs[variableName] = variableValue;
        }

        /// <summary>
        /// Implement this method with no parameters that evaluates the expression to a double
        /// value.
        /// </summary>
        /// <returns>the evaluate value.</returns>
        public double Evaluate()
        {
            return this.Evaluate(this.root);
        }
    }
}
