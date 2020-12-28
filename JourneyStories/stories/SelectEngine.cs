﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace JourneyStories.stories
{
    public class Node<T>
    {
        public int Id;
        public List<T> Data;
        public Hashtable NodeTable;

        public Node(int id)
        {
            Id = id;
            Data = null;
            NodeTable = new Hashtable();
        }

        public Node<T> AddNode(int nId)
        {
            Node<T> node = new Node<T>(nId);
            NodeTable.Add(nId, node);
            return node;
        }

        public Node<T> GetNode(int nId)
        {
            if (NodeTable.Contains(nId))
            {
                Object o = NodeTable[nId];
                return (Node<T>) o;
            }

            return null;
        }

        public void AddData(List<T> data)
        {
            if (Data == null)
            {
                Data = new List<T>();
            }

            foreach (T d in data)
            {
                Data.Add(d);
            }
        }

        public List<T> GetData()
        {
            return Data;
        }
    }

    public class MapTable<T>
    {
        private Hashtable _hashtable;

        public MapTable()
        {
            _hashtable = new Hashtable();
        }

        public void Put(T condition, int value)
        {
            _hashtable.Add(condition, value);
        }

        public int GetID(T condition)
        {
            if (_hashtable.Contains(condition))
            {
                Object o = _hashtable[condition];
                return (int) o;
            }

            return 0;
        }
    }

    public class SelectEngine<T>
    {
        private Node<T> _root;
        private List<Node<T>> current;
        private List<Node<T>> temp;

        public SelectEngine()
        {
            _root = new Node<T>(0);
            current = new List<Node<T>>();
            temp = new List<Node<T>>();
            current.Add(_root);
        }

        public MapTable<C> RegisterCondition<C>(List<C> conditions)
        {
            MapTable<C> mapTable = new MapTable<C>();
            for (int i = 0; i < conditions.Count; i++)
            {
                mapTable.Put(conditions[i], i);
            }

            return mapTable;
        }

        public Node<T> AddCondition(int conditionID)
        {
            Node<T> n = _root.GetNode(conditionID);
            if (null == n)
            {
                n = _root.AddNode(conditionID);
            }

            return n;
        }

        public Node<T> AddCondition(Node<T> node, int conditionID)
        {
            Node<T> n = node.GetNode(conditionID);
            if (null == n)
            {
                n = node.AddNode(conditionID);
            }

            return n;
        }

        public SelectEngine<T> StartSelect()
        {
            current.Clear();
            temp.Clear();
            temp.Add(_root);
            return this;
        }

        public SelectEngine<T> FindInCondition(int conditionID)
        {
            foreach (Node<T> node in current)
            {
                Node<T> find = node.GetNode(conditionID);
                if (find != null)
                {
                    temp.Add(find);
                }
            }

            return this;
        }

        public SelectEngine<T> FindInNextCondition(int conditionID)
        {
            List<Node<T>> t = current;
            current = temp;
            temp = t;
            temp.Clear();
            foreach (Node<T> node in current)
            {
                Node<T> find = node.GetNode(conditionID);
                if (find != null)
                {
                    temp.Add(find);
                }
            }

            return this;
        }

        public List<T> GetSelectedData()
        {
            List<T> data = new List<T>();
            foreach (Node<T> node in temp)
            {
                List<T> d = node.GetData();
                if (d != null)
                {
                    foreach (T l in d)
                    {
                        data.Add(l);
                    }
                }
            }

            return data;
        }
    }
}