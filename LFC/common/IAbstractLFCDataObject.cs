using System;
namespace SilverlightLFC.common
{
    public interface IAbstractLFCDataObject
    {
        void ClearObjectID();
        event LynxCommonProcessHandler DataObjectProcComplete;
        string getDatabaseIDFieldName();
        void getPath(System.Collections.Generic.List<AbstractLFCDataObject> PathList);
        string getUnAvailableIDValue();
        void InitSubObjectEvent();
        bool IsAvailableObjectID();
        bool IsDependenceListChanged(System.Collections.Generic.List<AbstractLFCDataObject> lol);
        bool IsLoaded();
        bool IsRelationObjectChanged(AbstractLFCDataObject lo);
        event LFCObjectChanged ObjctChanged;
        string ObjectID { get; set; }
        event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        void sendObjctChanged(LFCObjChanged f, string n);
        void sendObjctChanged(System.Reflection.FieldInfo f, object v);
        void sendProcEvent(bool r, string s, object o);
        DataOperation DataFlag { get; set; }
    }
}
