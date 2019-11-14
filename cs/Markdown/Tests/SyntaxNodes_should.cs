﻿using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Markdown.Languages;
using Markdown.Tree;
using NUnit.Framework;

namespace Markdown.Tests
{
    public class SyntaxNodes_should
    {
        [TestCase(typeof(Html))]
        [TestCase(typeof(MarkDown))]
        public void TextNodeShould_ConvertTo_WhenAnyLanguage_ReturnValue(Type type)
        {
            var value = "aba aba";
            var textNode = new TextNode(value);
            textNode.ConvertTo((ILanguage)type.GetConstructor(new Type[0])?.Invoke(new object[0]))
                .Should().Be(value);
        }
        
        [TestCase(typeof(Html),TagType.Em)]
        [TestCase(typeof(Html),TagType.Strong)]
        [TestCase(typeof(MarkDown),TagType.Em)]
        [TestCase(typeof(MarkDown),TagType.Strong)]
        public void TagNodeShould_ConvertTo_WhenAnyLanguageAndTextNode_AddInStartAndEndTag(Type type, TagType tagType)
        {
            var value = "aba aba";
            var tagNode = new TagNode(tagType,new List<SyntaxNode>(){new TextNode(value)});
            var el = (ILanguage) type.GetConstructor(new Type[0])?.Invoke(new object[0]);
            var result = tagNode.ConvertTo(el);
            result.Should().StartWith(el.Tags[tagType].Start);
            result.Should().EndWith(el.Tags[tagType].End);
        }
        
        [TestCase(typeof(Html))]
        [TestCase(typeof(MarkDown))]
        public void SyntaxTreeShould_ConvertTo_WhenAnyLanguageAndTwoTextNode_JoinText(Type type)
        {
            var value = "aba aba";
            var textNode = new TextNode(value);
            var syntaxTree = new SyntaxTree(new List<SyntaxNode>(){textNode,textNode});
            var el = (ILanguage) type.GetConstructor(new Type[0])?.Invoke(new object[0]);
            var result = syntaxTree.ConvertTo(el);
            result.Should().Be(value + value);
        }
        
        [TestCase(typeof(Html),TagType.Em)]
        [TestCase(typeof(Html),TagType.Strong)]
        [TestCase(typeof(MarkDown),TagType.Em)]
        [TestCase(typeof(MarkDown),TagType.Strong)]
        public void SyntaxTreeShould_ConvertTo_WhenAnyLanguageAndTextNodeAndTagNode_JoinText(Type type, TagType tagType)
        {
            var value = "aba aba";
            var textNode = new TextNode(value);
            var tagNode = new TagNode(tagType,new List<SyntaxNode>(){textNode});
            var syntaxTree = new SyntaxTree(new List<SyntaxNode>(){tagNode,textNode});
            var el = (ILanguage) type.GetConstructor(new Type[0])?.Invoke(new object[0]);
            var result = syntaxTree.ConvertTo(el);
            result.Should().Be(el.Tags[tagType].Start + value + el.Tags[tagType].End + value);
        }
        
        [TestCase(typeof(Html),TagType.Em, null)]
        [TestCase(typeof(Html),TagType.Strong, null)]
        [TestCase(typeof(MarkDown),TagType.Em,null)]
        [TestCase(typeof(MarkDown),TagType.Strong, null)]
        [TestCase(typeof(Html),TagType.Em, "")]
        [TestCase(typeof(Html),TagType.Strong, "")]
        [TestCase(typeof(MarkDown),TagType.Em,"")]
        [TestCase(typeof(MarkDown),TagType.Strong, "")]
        public void TagNodeShould_ConvertTo_WhenAnyLanguageAndTagNodeWithNullOrEmpty_ReturnEmptyWithTag(Type type, TagType tagType, string str)
        {
            var textNode = new TextNode(null);
            var tagNode = new TagNode(tagType,new List<SyntaxNode>(){textNode});
            var el = (ILanguage) type.GetConstructor(new Type[0])?.Invoke(new object[0]);
            var result = tagNode.ConvertTo(el);
            result.Should().Be(el.Tags[tagType].Start + el.Tags[tagType].End);
        }
    }
}