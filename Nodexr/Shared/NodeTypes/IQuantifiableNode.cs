﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nodexr.Shared.Nodes;
using Nodexr.Shared.NodeInputs;

namespace Nodexr.Shared.NodeTypes
{
    public interface IQuantifiableNode
    {
        InputDropdown<Reps> InputCount { get; }
        InputNumber InputNumber { get; }
        InputRange InputRange { get; }

        public enum Reps
        {
            One,
            ZeroOrMore,
            OneOrMore,
            ZeroOrOne,
            Number,
            Range
        }

        public static readonly Dictionary<Reps, string> displayNames = new Dictionary<Reps, string>()
        {
            {Reps.One, "One"},
            {Reps.ZeroOrMore, "Zero or more"},
            {Reps.OneOrMore, "One or more"},
            {Reps.ZeroOrOne, "Zero or one"},
            {Reps.Number, "Number"},
            {Reps.Range, "Range"}
        };

        public string GetSuffix(Reps mode, int? number = 0, int? min = 0, int? max = 0)
        {
            return mode switch
            {
                Reps.One => "",
                Reps.ZeroOrMore => "*",
                Reps.OneOrMore => "+",
                Reps.ZeroOrOne => "?",
                Reps.Number => $"{{{number ?? 0}}}",
                Reps.Range => $"{{{min ?? 0},{max}}}",
                _ => throw new ArgumentOutOfRangeException(nameof(mode))
            };
        }

        public static string GetSuffix(IQuantifiableNode node)
        {
            return node.InputCount.Value switch
            {
                Reps.One => "",
                Reps.ZeroOrMore => "*",
                Reps.OneOrMore => "+",
                Reps.ZeroOrOne => "?",
                Reps.Number => $"{{{node.InputNumber.InputContents ?? 0}}}",
                Reps.Range => $"{{{node.InputRange.Min ?? 0},{node.InputRange.Max}}}",
                _ => "",
            };
        }
    }
}
