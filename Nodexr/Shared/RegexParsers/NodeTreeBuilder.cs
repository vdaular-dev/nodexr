﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nodexr.Shared.NodeTypes;
using Nodexr.Shared;
using Nodexr.Shared.Nodes;
using Nodexr.Shared.NodeInputs;

namespace Nodexr.Shared.RegexParsers
{
    public class NodeTreeBuilder
    {
        private readonly NodeTree tree;
        private readonly Node endNode;
        private readonly List<int> columnHeights = new();
        private const int SpacingX = 250;
        private const int SpacingY =20;
        private static readonly Vector2 outputPos = new(1000, 300);

        public NodeTreeBuilder(Node endNode)
        {
            this.endNode = endNode;
            tree = new NodeTree();
        }

        public NodeTree Build()
        {
            FillColumns(endNode);
            return tree;
        }

        private void FillColumns(Node endNode)
        {
            tree.AddNode(endNode);
            int startHeight = (int)outputPos.y;
            AddToColumn(endNode, startHeight, 0);
            AddNodeChildren(endNode, startHeight, 1);
        }

        private static List<Node> GetChildren(Node node)
        {
            var inputs = node.GetAllInputs().OfType<InputProcedural>();
            var children = inputs.Select(input => input.ConnectedNode).OfType<Node>().ToList();
            return children;
        }

        private void AddNodeChildren(Node parent, int pos, int column)
        {
            var children = GetChildren(parent);
            int childrenHeight = children.Skip(1).Select(node => node.GetHeight() + SpacingY).Sum();
            int startPos = pos - (childrenHeight / 2);
            foreach (var child in children)
            {
                tree.AddNode(child);
                int childPos = AddToColumn(child, startPos, column);
                AddNodeChildren(child, childPos, column + 1);
            }
        }

        private int AddToColumn (Node node, int pos, int column)
        {
            if(columnHeights.Count <= column)
            {
                //Assumes that no columns are skipped
                columnHeights.Add(int.MinValue);
            }

            if (columnHeights[column] < pos)
            {
                //Leave empty spaces when appropriate
                columnHeights[column] = pos;
            }

            double xPos = outputPos.x - (column * SpacingX);
            int yPos = columnHeights[column];
            columnHeights[column] += node.GetHeight() + SpacingY;

            node.Pos = new Vector2(xPos, yPos);
            return yPos;
        }
    }
}
