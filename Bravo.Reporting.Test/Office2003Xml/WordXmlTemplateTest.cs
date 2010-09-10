﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Xml;
using System.Xml.Schema;

using NUnit.Framework;

using Bravo.Reporting.Test;

namespace Bravo.Reporting.Office2003Xml.Test
{
    [TestFixture(Description = "Word 2003 XML 格式模板的测试")]
    public sealed class WordXmlTemplateTest
    {
        private static void AssertValidWordMLDocument(XmlDocument xml)
        {
            var xsdFiles = new string[] 
            {
                @"resources/schemas/word2003/wordnet.xsd",
                @"resources/schemas/word2003/wordnetaux.xsd",
                @"resources/schemas/word2003/wordsp.xsd",
                @"resources/schemas/word2003/office.xsd",
                @"resources/schemas/word2003/vml.xsd",
                @"resources/schemas/word2003/aml.xsd",
                @"resources/schemas/word2003/xsdlib.xsd",
                @"resources/schemas/word2003/w10.xsd",
            };

            TemplateTestHelper.AssertValidXmlDocument(xml, xsdFiles);
        }

        [Test(Description = "测试 Word 2003 Xml 的简单引用替换")]
        public void TestReferenceReplacement()
        {
            var ctx = new Dictionary<string, object>()
            {
                {"var1", "_HELLO_" },
                {"var2", "_WORLD_" },
            };

            var result = TemplateTestHelper.RenderTemplate<WordMLDocument>(
                @"resources/word2003xml_docs/template_reference_replacement.xml", ctx);

            var xmldoc = TemplateTestHelper.GetlXmlDocument((WordMLDocument)result);
            AssertValidWordMLDocument(xmldoc);

            var body = xmldoc.GetElementsByTagName("w:body")[0];
            var bodyText = body.InnerText.Trim();

            Assert.AreEqual("TEST_HELLO_REFERENCE_WORLD_REPLACEMENT", bodyText);
        }

        [Test(Description = "测试 Word 2003 Xml 的 RTL:// 链接 URL 转义")]
        public void TestEscapeUrl()
        {
            var ctx = new Dictionary<string, object>()
            {
            };

            var result = TemplateTestHelper.RenderTemplate<WordMLDocument>(
                @"resources/word2003xml_docs/template_escape_url.xml", ctx);

            var xmldoc = TemplateTestHelper.GetlXmlDocument((WordMLDocument)result);
            AssertValidWordMLDocument(xmldoc);

            var body = xmldoc.GetElementsByTagName("w:body")[0];
            var bodyText = body.InnerText.Trim();

            Assert.AreEqual("革命", bodyText);
        }

    }
}