using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace BlogSite.Components
{
    public partial class OutlineBox : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // 页面加载时可以初始化TreeView
        }

        /// <summary>
        /// 接受HTML片段，分析其中的有序列表标签，生成TreeView大纲，并返回插入锚点的HTML片段
        /// </summary>
        /// <param name="htmlFragment">输入的HTML片段</param>
        /// <returns>插入锚点的HTML片段</returns>
        public string GenerateOutline(string html)
        {
            // 解析 HTML 片段
            var document = XDocument.Parse($"<root>{html}</root>"); // 包装成有效的 XML

            TreeView1.Nodes.Clear(); // 清空现有节点

            // 用于生成唯一的锚点 ID
            int anchorIndex = 1;

            // 遍历所有一级、二级有序列表
            foreach (var ol in document.Descendants("ol"))
            {
                foreach (var li in ol.Elements("li"))
                {
                    // 为每个列表项生成锚点
                    var anchorId = $"anchor-{anchorIndex++}";
                    li.AddFirst(new XElement("a", new XAttribute("id", anchorId)));

                    // 将列表项的文本添加到 TreeView 节点
                    var textContent = GetDirectText(li);
                    var node = new TreeNode(textContent, anchorId);

                    // 如果父节点存在，将其作为子节点添加
                    var parentOl = li.Parent?.Parent;
                    if (parentOl != null && parentOl.Name == "li")
                    {
                        var parentAnchorId = parentOl.Element("a")?.Attribute("id")?.Value;
                        var parentNode = FindTreeNodeByValue(TreeView1.Nodes, parentAnchorId);
                        parentNode?.ChildNodes.Add(node);
                    }
                    else
                    {
                        // 否则将其作为根节点添加
                        TreeView1.Nodes.Add(node);
                    }
                }
            }

            // 返回处理后的 HTML 片段
            return document.Root?.ToString();
        }
        private string GetDirectText(XElement element)
        {
            // 提取直接文本内容，忽略子标签
            string res = element.Nodes().OfType<XText>().FirstOrDefault()?.Value.Trim() ?? string.Empty;
            if (res != string.Empty)
            {
                int cutIndex = res.IndexOf(" ");
                if(cutIndex > 0) res = res.Substring(0, cutIndex);
            }
            return res;
        }

        private TreeNode FindTreeNodeByValue(TreeNodeCollection nodes, string value)
        {
            foreach (TreeNode node in nodes)
            {
                if (node.Value == value)
                {
                    return node;
                }

                var childNode = FindTreeNodeByValue(node.ChildNodes, value);
                if (childNode != null)
                {
                    return childNode;
                }
            }
            return null;
        }

        protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
        {
            // 处理TreeView节点选择事件
            var selectedNode = TreeView1.SelectedNode;
            if (selectedNode != null)
            {
                // 跳转到对应的锚点
                Response.Redirect(Request.Url + "#" + selectedNode.Value);
            }
        }
    }
}