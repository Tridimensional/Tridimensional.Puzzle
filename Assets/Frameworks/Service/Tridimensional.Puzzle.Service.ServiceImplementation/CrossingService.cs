using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Tridimensional.Puzzle.Foundation.Entity;
using Tridimensional.Puzzle.Foundation.Enumeration;
using Tridimensional.Puzzle.Service.Contract;
using Tridimensional.Puzzle.Service.IServiceProvider;
using UnityEngine;

namespace Tridimensional.Puzzle.Service.ServiceImplementation
{
    public class CrossingService : ICrossingService
    {
        #region CrossingTreeSingleton

        private static MultiTree<CrossContract> _treeInstance;

        private static MultiTree<CrossContract> TreeInstance
        {
            get
            {
                if (_treeInstance == null)
                {
                    var textAsset = Resources.Load("Config/Crossing") as TextAsset;
                    using (var memoryStream = new MemoryStream(textAsset.bytes))
                    {
                        var xmlDocument = new XmlDocument();
                        xmlDocument.Load(memoryStream);
                        _treeInstance = GetTreeInstance(xmlDocument);
                    }
                }

                return _treeInstance;
            }
        }

        private static MultiTree<CrossContract> GetTreeInstance(XmlDocument xmlDocument)
        {
            return GetTreeInstance(xmlDocument.DocumentElement.ChildNodes[0]);
        }

        private static MultiTree<CrossContract> GetTreeInstance(XmlNode xmlNode)
        {
            var buttons = GetButtonContracts(xmlNode.Attributes["buttons"].Value.Split(new[] { ',' }));
            var frames = GetFrameContracts(xmlNode.Attributes["frames"].Value.Split(new[] { ',' }));

            var multiTree = new MultiTree<CrossContract> { Value = new CrossContract { Buttons = buttons, Frames = frames } };

            foreach (XmlNode subNode in xmlNode.ChildNodes) { multiTree.Append(GetTreeInstance(subNode)); }

            return multiTree;
        }

        private static FrameContract[] GetFrameContracts(string[] array)
        {
            if (array == null || array.Length == 0) { return null; }

            var list = new List<FrameContract>();

            for (var i = 0; i < array.Length; i++)
            {
                list.Add(((FrameCategory)Enum.Parse(typeof(FrameCategory), array[i], true)).ToFrameContract());
            }

            return list.ToArray();
        }

        private static ButtonContract[] GetButtonContracts(string[] array)
        {
            if (array == null || array.Length == 0) { return null; }

            var list = new List<ButtonContract>();

            for (var i = 0; i < array.Length; i++)
            {
                list.Add(((ButtonCategory)Enum.Parse(typeof(ButtonCategory), array[i], true)).ToButtonContract());
            }

            return list.ToArray();
        }

        #endregion

        public MultiTree<CrossContract> GetConfigurationTree()
        {
            return TreeInstance;
        }
    }
}
