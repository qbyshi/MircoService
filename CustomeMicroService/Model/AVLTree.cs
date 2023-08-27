using System;

public class AVLTree<T> where T : IComparable<T>
{
    private class Node
    {
        public T Data;
        public Node Left;
        public Node Right;
        public int Height;
    }

    private Node root;

    // Insert a node into the AVL Tree
    public void Insert(T data)
    {
        root = Insert(root, data);
    }

    private Node Insert(Node node, T data)
    {
        if (node == null)
        {
            return new Node { Data = data, Height = 1 };
        }

        int compareResult = data.CompareTo(node.Data);

        if (compareResult < 0)
        {
            node.Left = Insert(node.Left, data);
        }
        else if (compareResult > 0)
        {
            node.Right = Insert(node.Right, data);
        }
        else
        {
            // Duplicate data, do nothing
            return node;
        }

        // Update height
        node.Height = Math.Max(Height(node.Left), Height(node.Right)) + 1;

        // Perform rotations to maintain balance
        int balance = GetBalance(node);

        // Left Heavy
        if (balance > 1)
        {
            if (data.CompareTo(node.Left.Data) < 0)
            {
                return RightRotate(node); // Single right rotation
            }
            else
            {
                node.Left = LeftRotate(node.Left); // Left rotation on left child
                return RightRotate(node); // Single right rotation
            }
        }

        // Right Heavy
        if (balance < -1)
        {
            if (data.CompareTo(node.Right.Data) > 0)
            {
                return LeftRotate(node); // Single left rotation
            }
            else
            {
                node.Right = RightRotate(node.Right); // Right rotation on right child
                return LeftRotate(node); // Single left rotation
            }
        }

        return node;
    }

    // Delete a node from the AVL Tree
    public void Delete(T data)
    {
        root = Delete(root, data);
    }

    private Node Delete(Node node, T data)
    {
        // Standard BST delete operation
        if (node == null)
        {
            return node;
        }

        int compareResult = data.CompareTo(node.Data);

        if (compareResult < 0)
        {
            node.Left = Delete(node.Left, data);
        }
        else if (compareResult > 0)
        {
            node.Right = Delete(node.Right, data);
        }
        else
        {
            // Node to be deleted found

            // Node with one child or no child
            if ((node.Left == null) || (node.Right == null))
            {
                Node temp = null;
                if (temp == node.Left)
                {
                    temp = node.Right;
                }
                else
                {
                    temp = node.Left;
                }

                // No child case
                if (temp == null)
                {
                    temp = node;
                    node = null;
                }
                else
                {
                    node = temp; // Copy the contents of the non-empty child
                }
            }
            else
            {
                // Node with two children: Get the inorder successor (smallest
                // in the right subtree)
                Node temp = MinValueNode(node.Right);

                // Copy the inorder successor's data to this node
                node.Data = temp.Data;

                // Delete the inorder successor
                node.Right = Delete(node.Right, temp.Data);
            }
        }

        if (node == null)
        {
            return node;
        }

        // Update height
        node.Height = Math.Max(Height(node.Left), Height(node.Right)) + 1;

        // Perform rotations to maintain balance
        int balance = GetBalance(node);

        // Left Heavy
        if (balance > 1)
        {
            if (GetBalance(node.Left) >= 0)
            {
                return RightRotate(node); // Single right rotation
            }
            else
            {
                node.Left = LeftRotate(node.Left); // Left rotation on left child
                return RightRotate(node); // Single right rotation
            }
        }

        // Right Heavy
        if (balance < -1)
        {
            if (GetBalance(node.Right) <= 0)
            {
                return LeftRotate(node); // Single left rotation
            }
            else
            {
                node.Right = RightRotate(node.Right); // Right rotation on right child
                return LeftRotate(node); // Single left rotation
            }
        }

        return node;
    }

    // Get the rank of a given data element
    public int GetRank(T data)
    {
        return GetRank(root, data);
    }

    private int GetRank(Node node, T data)
    {
        if (node == null)
        {
            return 0; // Data not found
        }

        int compareResult = data.CompareTo(node.Data);

        if (compareResult < 0)
        {
            return GetRank(node.Left, data);
        }
        else if (compareResult > 0)
        {
            return 1 + GetSize(node.Left) + GetRank(node.Right, data);
        }
        else
        {
            return 1 + GetSize(node.Left); // Data found
        }
    }

    public SortedSet<T> GetElementsByRankRange(int startRank, int endRank)
    {
        if (startRank < 1 || startRank > GetSize(root) || endRank < startRank)
        {
            throw new ArgumentException("Invalid rank range");
        }

        SortedSet<T> elements = new SortedSet<T>();
        GetElementsByRankRange(root, startRank, endRank, elements);
        return elements;
    }

    private void GetElementsByRankRange(Node node, int startRank, int endRank, SortedSet<T> elements)
    {
        if (node == null || elements.Count >= endRank)
        {
            return;
        }

        int leftSize = GetSize(node.Left);
        int currentNodeRank = leftSize + 1;

        if (currentNodeRank >= startRank && currentNodeRank <= endRank)
        {
            elements.Add(node.Data);
        }

        if (currentNodeRank < endRank)
        {
            GetElementsByRankRange(node.Right, startRank - currentNodeRank, endRank - currentNodeRank, elements);
        }

        if (currentNodeRank >= startRank)
        {
            GetElementsByRankRange(node.Left, startRank, endRank, elements);
        }
    }

    // Get the element at a given rank
    public T GetElementByRank(int rank)
    {
        if (rank < 1 || rank > GetSize(root))
        {
            throw new ArgumentException("Invalid rank");
        }

        return GetElementByRank(root, rank);
    }

    private T GetElementByRank(Node node, int rank)
    {
        if (node == null)
        {
            return default(T);
        }

        int leftSize = GetSize(node.Left);

        if (rank <= leftSize)
        {
            return GetElementByRank(node.Left, rank);
        }
        else if (rank == leftSize + 1)
        {
            return node.Data;
        }
        else
        {
            return GetElementByRank(node.Right, rank - leftSize - 1);
        }
    }

    // Get the size (number of nodes) of the tree rooted at the given node
    private int GetSize(Node node)
    {
        if (node == null)
        {
            return 0;
        }
        return 1 + GetSize(node.Left) + GetSize(node.Right);
    }

    // Height of a node (null nodes have height -1)
    private int Height(Node node)
    {
        if (node == null)
        {
            return 0;
        }
        return node.Height;
    }

    // Get balance factor of a node
    private int GetBalance(Node node)
    {
        if (node == null)
        {
            return 0;
        }
        return Height(node.Left) - Height(node.Right);
    }

    // Perform a left rotation
    private Node LeftRotate(Node node)
    {
        Node newRoot = node.Right;
        node.Right = newRoot.Left;
        newRoot.Left = node;
        node.Height = Math.Max(Height(node.Left), Height(node.Right)) + 1;
        newRoot.Height = Math.Max(Height(newRoot.Left), Height(newRoot.Right)) + 1;
        return newRoot;

    }

    // Perform a right rotation
    private Node RightRotate(Node node)
    {
        Node newRoot = node.Left;
        node.Left = newRoot.Right;
        newRoot.Right = node;
        node.Height = Math.Max(Height(node.Left), Height(node.Right)) + 1;
        newRoot.Height = Math.Max(Height(newRoot.Left), Height(newRoot.Right)) + 1;
        return newRoot;
    }

    // Find the node with the minimum value in the tree rooted at the given node
    private Node MinValueNode(Node node)
    {
        Node current = node;
        while (current.Left != null)
        {
            current = current.Left;
        }
        return current;
    }
}
