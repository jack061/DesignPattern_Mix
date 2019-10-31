using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 二叉树遍历
{
    class Program
    {
        static void Main(string[] args)
        {
            Tree tree = CreatFakeTree();
            PostOrder(tree);
            Console.ReadKey();

        }
        //先序遍历
        public static void PreOrder(Tree tree)
        {
            if (tree == null)
                return;

            System.Console.WriteLine(tree.Value);
            PreOrder(tree.Left);
            PreOrder(tree.Right);
        }

        //中序遍历
        public static void InOrder(Tree tree)
        {
            if (tree == null)
            {
                return;
            }
            InOrder(tree.Left);
            Console.WriteLine(tree.Value);
            InOrder(tree.Right);
        }
        //后序遍历
        public static void PostOrder(Tree tree)
        {
            if (tree == null)
            {
                return;
            }
            PostOrder(tree.Left);
            PostOrder(tree.Right);
            Console.WriteLine(tree.Value);
        }
        //创建二叉树
        public static Tree CreatFakeTree()
        {
            Tree tree = new Tree() { Value = "A" };
            tree.Left = new Tree()
            {
                Value = "B",
                Left = new Tree() { Value = "D", Left = new Tree() { Value = "G" } },
                Right = new Tree() { Value = "E", Right = new Tree() { Value = "H" } }
            };
            tree.Right = new Tree() { Value = "C", Left = new Tree() { Value = "K" }, Right = new Tree() { Value = "F" } };
            return tree;
        }
    }
    public class Tree
    {
        public string Value;
        public Tree Left;
        public Tree Right;
    }
}
