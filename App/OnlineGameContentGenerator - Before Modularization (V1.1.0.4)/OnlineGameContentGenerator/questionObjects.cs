using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

///Contains the structures to contain question information
namespace OnlineGameContentGenerator
{
    //Base question object that contains all generic properties
    [Serializable]
    public class questionObjectBase
    {
        public string questionNumber;
        public string questionText;
        public string questionType;
        public int weight;
    }

    //Simple question that extends the base to provide functionality for games like Multiple choice and T&F
    [Serializable]
    public class questionObjectSimple : questionObjectBase
    {
        public List<questionItemSimple> questionItems = new List<questionItemSimple>();
    }

    //The barebones of what a question item could be, just the text
    [Serializable]
    public class questionItemBase
    {
        public string itemText;
    }

    //Extends questionItemBase and adds functionality for Multiple choice and T&F
    [Serializable]
    public class questionItemSimple : questionItemBase
    {
        public bool correct;
        public popups popups;
    }

    //Extention to be used when wishing to implement popups into a question item.
    [Serializable]
    public class popups
    {
        public bool popupEnabled;
        public string popupTitle;
        public string popupBody;
    }

    //A question that extends the base to provide functionality for games like Drag & Drop
    [Serializable]
    public class questionObjectComplex : questionObjectBase
    {
        public List<itemContainer> questionItems;
    }

    //Provides required properties to use containers for items like in Drag & Drop.
    [Serializable]
    public class itemContainer
    {
        public string itemContainerLabel;
        public List<questionItemBase> items;
    }
}
