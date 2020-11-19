using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Util
{
    public static class UnitySearch
    {
        public static float Y;
         public static GameObject GetParent(this GameObject gm)
        {
            return gm.transform.parent.gameObject;
        }
        
        public static GameObject GetChildByName(this GameObject gm, string name)
        {
            for (var i = 0; i < gm.transform.childCount; i++)
            {
                if (gm.transform.GetChild(i).name == name)
                {
                    return gm.transform.GetChild(i).gameObject;
                }
            }
            return null;
        }

        public static void SwapChildren(this GameObject gm, GameObject otherGm)
        {
            var a = gm.transform.GetSiblingIndex();
            var b = otherGm.transform.GetSiblingIndex();
            gm.transform.SetSiblingIndex(b);
            otherGm.transform.SetSiblingIndex(a);
        }
        
        public static GameObject GetChild(this GameObject gm, string query)
        {
            var childs = GetChilds(gm, query);
            return childs.Count > 0 ? childs[0] : null;
        }
        
        public static List<GameObject> GetChilds(this GameObject gm, string query, List<GameObject> externalList = null)
        {
            // Парсим запрос и удаляем лишнее
            var queryTuple = query.Split('>').Select(x => x.Trim()).ToList();
            var queryLength = queryTuple.Count;
            var levelFirst = queryTuple.First();
            queryTuple.Remove(queryTuple.First());
            
            // Собираем чилды по имени
            if (externalList != null)
            {
                var list = new List<GameObject>();
                for (var i = 0; i < externalList.Count; i++)
                {
                    for (var j = 0; j < externalList[i].transform.childCount; j++)
                    {
                        if (Regex.IsMatch(externalList[i].transform.GetChild(j).name, levelFirst))
                            list.Add(externalList[i].transform.GetChild(j).gameObject);
                    }
                }
                externalList = list;
                if (queryLength == 1) return list;
            }
            else
            {
                var list = new List<GameObject>();
                for (var i = 0; i < gm.transform.childCount; i++)
                    if (Regex.IsMatch(gm.transform.GetChild(i).name, levelFirst))
                        list.Add(gm.transform.GetChild(i).gameObject);
                externalList = list;
                if (queryLength == 1) return list;
            }
            
            return GetChilds(gm, string.Join(">", queryTuple), externalList);
        }

        public static List<GameObject> GetAllChilds(this GameObject gm, bool withHierarchy = false)
        {
            var list = new List<GameObject>();
            for (var i = 0; i < gm.transform.childCount; i++)
            {
                list.Add(gm.transform.GetChild(i).gameObject);
                if (withHierarchy && gm.transform.GetChild(i).gameObject.transform.childCount > 0)
                {
                    list.AddRange(gm.transform.GetChild(i).gameObject.GetAllChilds(true));
                }
            }
            return list;
        }
        
        public static void OnceClick(this GameObject gm, UnityAction<GameObject> method)
        {
            gm.GetComponent<Button>().onClick.RemoveAllListeners();
            gm.OnClick(method);
        }
        
        public static void OnClick(this GameObject gm, UnityAction<GameObject> method)
        {
            gm.GetComponent<Button>().onClick.AddListener(() => { method.Invoke(gm); });
        }
        public static void OnClick(this GameObject gm)
        {
            gm.GetComponent<Button>().onClick.Invoke();
        }
        
        public static void OnClick(this List<GameObject> gm, UnityAction<GameObject> method)
        {
            for (var i = 0; i < gm.Count; i++)
            {
                var x = gm[i];
                gm[i].GetComponent<Button>().onClick.AddListener(() => { method.Invoke(x); });
            }
        }
        
        public static void OnceMouseOver(this GameObject gm, UnityAction<GameObject> method)
        {
            if (!gm.GetComponent<ButtonListener>())
                gm.AddComponent<ButtonListener>().OnceOverListener = () => method.Invoke(gm);
            else gm.GetComponent<ButtonListener>().OnceOverListener = () => method.Invoke(gm);
        }
        
        public static void OnceMouseOut(this GameObject gm, UnityAction<GameObject> method)
        {
            if (!gm.GetComponent<ButtonListener>())
                gm.AddComponent<ButtonListener>().OnceOutListener = () => method.Invoke(gm);
            else gm.GetComponent<ButtonListener>().OnceOutListener = () => method.Invoke(gm);
        }
        
        public static void OnceMouseDown(this GameObject gm, UnityAction<GameObject> method)
        {
            if (!gm.GetComponent<ButtonListener>())
                gm.AddComponent<ButtonListener>().OnceDownListener = () => method.Invoke(gm);
            else gm.GetComponent<ButtonListener>().OnceDownListener = () => method.Invoke(gm);
        }
        
        public static void OnceMouseUp(this GameObject gm, UnityAction<GameObject> method)
        {
            if (!gm.GetComponent<ButtonListener>())
                gm.AddComponent<ButtonListener>().OnceUpListener = () => method.Invoke(gm);
            else gm.GetComponent<ButtonListener>().OnceUpListener = () => method.Invoke(gm);
        }
        
        public static void OnMouseOver(this GameObject gm, UnityAction<GameObject> method)
        {
            if (!gm.GetComponent<ButtonListener>())
                gm.AddComponent<ButtonListener>().AddHoverEnterListener += data => method.Invoke(gm);
            else gm.GetComponent<ButtonListener>().AddHoverEnterListener += data => method.Invoke(gm);
        }
        public static void OnMouseOver(this List<GameObject> gm, UnityAction<GameObject> method)
        {
            for (var i = 0; i < gm.Count; i++)
            {
                var x = gm[i];
                if (!gm[i].GetComponent<ButtonListener>())
                    gm[i].AddComponent<ButtonListener>().AddHoverEnterListener += data => method.Invoke(x);
                else gm[i].GetComponent<ButtonListener>().AddHoverEnterListener += data => method.Invoke(x);
            }
        }
        
        public static void OnMouseOut(this GameObject gm, UnityAction<GameObject> method)
        {
            if (!gm.GetComponent<ButtonListener>())
                gm.AddComponent<ButtonListener>().AddHoverExitListener += data => method.Invoke(gm);
            else gm.GetComponent<ButtonListener>().AddHoverExitListener += data => method.Invoke(gm);
        }
        public static void OnMouseOut(this List<GameObject> gm, UnityAction<GameObject> method)
        {
            for (var i = 0; i < gm.Count; i++)
            {
                var x = gm[i];
                if (!gm[i].GetComponent<ButtonListener>())
                    gm[i].AddComponent<ButtonListener>().AddHoverExitListener += data => method.Invoke(x);
                else gm[i].GetComponent<ButtonListener>().AddHoverExitListener += data => method.Invoke(x);
            }
        }
        
        public static void OnMouseDown(this GameObject gm, UnityAction<GameObject> method)
        {
            if (!gm.GetComponent<ButtonListener>())
                gm.AddComponent<ButtonListener>().AddDownListener += data => method.Invoke(gm);
            else gm.GetComponent<ButtonListener>().AddDownListener += data => method.Invoke(gm);
        }
        
        public static void OnMouseUp(this GameObject gm, UnityAction<GameObject> method)
        {
            if (!gm.GetComponent<ButtonListener>())
                gm.AddComponent<ButtonListener>().AddUpListener += data => method.Invoke(gm);
            else gm.GetComponent<ButtonListener>().AddUpListener += data => method.Invoke(gm);
        }
        
        public static void SetMaterial(this GameObject gm, Material material)
        {
            if (gm.GetComponent<Renderer>()) gm.GetComponent<Renderer>().material = material;
            
        }
        public static void SetMaterial(this List<GameObject> gm, Material material)
        {
            for (var i = 0; i < gm.Count; i++)
            {
                if (gm[i].GetComponent<Renderer>()) gm[i].GetComponent<Renderer>().material = material;
            }
        }
        public static void SetActive(this List<GameObject> gm, bool status)
        {
            for (var i = 0; i < gm.Count; i++) gm[i].SetActive(status);
        }
        public static void SetText(this GameObject gm, string text)
        {
            gm.GetComponent<Text>().text = text;
        }
        public static void SetText(this List<GameObject> gm, string text)
        {
            for (var i = 0; i < gm.Count; i++) gm[i].GetComponent<Text>().text = text;
        }
        public static void SetText(this List<GameObject> gm, string text, Color color)
        {
            for (var i = 0; i < gm.Count; i++)
            {
                gm[i].GetComponent<Text>().text = text;
                gm[i].GetComponent<Text>().color = color;
            }
        }
        public static void SetText(this List<GameObject> gm, Func<GameObject, string> method)
        {
            for (var i = 0; i < gm.Count; i++)
                gm[i].GetComponent<Text>().text = method.Invoke(gm[i]);
        }

        public static GameObject First(string query)
        {
            var element = Find(query);
            if (element.Count > 0) return element[0];
            return null;
        }
        
        public static List<GameObject> Find(string query)
        {
            var list = new List<GameObject>();

            if (query.Contains(">"))
            {
                var queryTuple = query.Split('>').Select(x => x.Trim()).ToList();
                var levelFirst = queryTuple.First();
                queryTuple.Remove(queryTuple.First());
                return GameObject.Find(levelFirst).GetChilds(string.Join(">", queryTuple));
            }
            
            list.Add(GameObject.Find(query));
            
            return list;
        }

        public static void FindAndDestroy(string query, Action<GameObject> destroy)
        {
            var items = Find(query);
            foreach (var item in items)
            {
                destroy(item);
            }
        }
    }
}