using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

public class WriteMap : MonoBehaviour {

	private string MapName;
	private int[,] toSave;
	private XmlWriter writer;

	// Use this for initialization
	void Start () {
	
	}
	void XMLWrite(string name, TDMap t)
	{
		MapName = name; 
		toSave = t.mapData;
		int x = 0;
		
		
		DirectoryInfo d = new DirectoryInfo(Environment.CurrentDirectory);
		string p = d.FullName + @"\Assets\Maps\";
		
		d = new DirectoryInfo(p);
		
		string originalName = name;
		
		FileInfo[] files = d.GetFiles();
		bool fileExists = false;
		
		for(int j = 0; j < files.Length; j++)
		{
			if(files[j].Name.Equals(MapName + ".xml"))
			{
				fileExists = true;
			}
		}
		
		while(fileExists)
		{
			MapName = originalName + "(" + x + ")";
			x++;
			fileExists = false;
			for (int j = 0; j < files.Length; j++)
			{
				if (files[j].Name.Equals(MapName + ".xml"))
				{
					fileExists = true;
				}
			}
			
		}
		
		
		XmlWriterSettings mySettings = new XmlWriterSettings();
		mySettings.Indent = true;
		mySettings.IndentChars = ("\t");
		mySettings.NewLineHandling = NewLineHandling.Entitize;
		writer = XmlWriter.Create(p + MapName + ".xml",mySettings);
	}
	
	public void Save()
	{
		
		writer.WriteStartDocument(true);
		
		writer.WriteStartElement("map");
		writer.WriteAttributeString("size", toSave.Length + "");
		writer.WriteAttributeString("id", MapName);
		writer.WriteStartElement("tiles");
		
		
		int x = 0;
		int y = 0;
		int tileID;
		int instanceOfID;
		int nextID;
		
		while (y < Mathf.Sqrt(toSave.Length))
		{
			
			tileID = toSave[y,x];
			instanceOfID = 1;
			if (x != (toSave.Length - 1))
			{
				nextID = toSave[y,x+1];
			}
			else if (y != (toSave.Length - 1))
			{
				nextID = toSave[(y + 1),x];
			}
			else
			{
				writer.WriteAttributeString("terrain", "EndOfFile");
				break;
			}
			
			while (nextID == tileID)
			{
				x++; instanceOfID++;
				
				if (x == toSave.Length)
				{ y++; x = 0; }
				
				if (y >= toSave.Length)
				{ break; }
				
				if (x != (toSave.Length - 1))
				{
					nextID = toSave[y,x + 1];
				}
				else if (y != (toSave.Length - 1))
				{
					nextID = toSave[(y + 1),0];
				}
				else
				{
					break;
				}
			}
			writer.WriteStartElement("tile");
			writer.WriteAttributeString("terrain", tileID.ToString());
			writer.WriteAttributeString("length", ""+instanceOfID);
			writer.WriteEndElement();
			x++;
			if (x == toSave.Length)
				y++; x = 0;
		}
		writer.WriteEndElement();
		
		writer.WriteEndDocument();
		writer.Close();
	}
}

