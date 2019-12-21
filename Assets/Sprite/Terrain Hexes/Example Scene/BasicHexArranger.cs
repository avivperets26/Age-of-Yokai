namespace dgbExamples
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEditor;

	public class BasicHexArranger : MonoBehaviour {
		public Sprite[] tile_dump;
		public Sprite[] random_decor;
		public Sprite underground;
		public Sprite underwater;
		public Sprite[] undervoid;

		void Start() {
			float tile_width = 2.56f;
			float tile_height = 2.56f; // sprite is 3.84f total
			float tile_under_height = 1.28f; 

			// start in upper-left corner, ish.
			float anchor_x = Camera.main.pixelWidth * 0.01f * -0.5f - tile_width * 0.5f;
			float anchor_y = Camera.main.pixelHeight * 0.01f * 0.5f - tile_height * 0.5f;
			float x = 0f;
			float y = 0f;
			int row = 1;
			int decor = 8; 

			foreach (Sprite s in tile_dump) {

				// tile sprite
				GameObject g = new GameObject (s.name);
				g.AddComponent (typeof(SpriteRenderer));
				g.GetComponent<SpriteRenderer> ().sprite = s;
				g.GetComponent<SpriteRenderer> ().sortingOrder = row;

				// underground sprite
				GameObject dirt = new GameObject (s.name + "_underground");
				dirt.AddComponent (typeof(SpriteRenderer));

				// assign proper under"ground" sprite
				if (s.name.Contains("Void")) {
					dirt.GetComponent<SpriteRenderer> ().sprite = undervoid[Random.Range(0,3)];
				}
				else if (s.name.Contains("Ocean")) {
					dirt.GetComponent<SpriteRenderer> ().sprite = underwater;
				} else {
					dirt.GetComponent<SpriteRenderer> ().sprite = underground;
				}
					
				dirt.GetComponent<SpriteRenderer> ().sortingOrder = row;

				float pos_x = anchor_x + x * tile_width;
				float pos_y = anchor_y + y * tile_height * 0.75f;

				if (row % 2 == 0) {
					// offset alternate rows
					pos_x += tile_width * 0.5f;
				}
					
				g.transform.position = new Vector3 (pos_x,pos_y,0f);

				dirt.transform.SetParent (g.transform);
				dirt.transform.position = new Vector3 (pos_x,pos_y + tile_under_height * 0.5f ,1f);


				if (decor > 0) {
					
					// place some random decor sprites in the first four hexes, just for fun.
					Sprite thing_sprite = random_decor[ Random.Range(0,random_decor.Length)];
					GameObject thing = new GameObject (thing_sprite.name + decor.ToString());
					thing.AddComponent (typeof(SpriteRenderer));
					thing.GetComponent<SpriteRenderer> ().sprite = thing_sprite;
					thing.GetComponent<SpriteRenderer> ().sortingOrder = row+1;

					thing.transform.SetParent (g.transform);
					thing.transform.position = new Vector3 (	Random.Range(pos_x - 0.64f, pos_x + 0.64f),
																Random.Range(pos_y + tile_height * 0.25f, pos_y + tile_height * 0.75f),
																1f);
					decor--;
				}

				x += 1f;

				// make rows of eight tiles across
				if (x >= 8f) {
					x -= 8f;
					y--;
					row++;
				}

			}
		}
	}
}