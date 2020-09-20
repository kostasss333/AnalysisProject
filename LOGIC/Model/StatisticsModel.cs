﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL.Entitites;

namespace LOGIC.Models
{
    public class StatisticsModel
    {
        public List<Article> articles = new List<Article>();
        public int average;
        public List<Book> books = new List<Book>();
        public Dictionary<string, int> dictionary;
        public List<Inbook> inbooks = new List<Inbook>();
        public List<Inproceeding> inproceedings = new List<Inproceeding>();
        public Dictionary<string, int> piedict;
        public List<Publishment> pubs = new List<Publishment>();
        public Dictionary<string, int> unitdict;
    }
}