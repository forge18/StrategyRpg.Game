using Godot;
using Godot.Collections;

namespace Data.Resources
{
    public class BaseLoader
    {
        public string[] ConvertResourcesToDataIdArray(Array<Resource> list)
        {
            var stringList = new string[list.Count];
            var index = 0;
            foreach (var item in list)
            {
                var dataId = (string)item.Call("GetDataId");
                stringList[index] = dataId;
                index++;
            }

            return stringList;
        }

        public string[] ConvertTagsArrayToSystemArray(Array<string> list)
        {
            var stringList = new string[list.Count];
            var index = 0;
            foreach (var item in list)
            {
                stringList[index] = item;
                index++;
            }

            return stringList;
        }
    }
}