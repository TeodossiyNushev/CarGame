using UnityEngine;

public class menu : MonoBehaviour
{

    public GameObject[] items;
    public GameObject itemsFolder;

    public int position = 0;
    public int length;

    public Vector3[] positions;
    public Vector3[] scale;

    void Start(){
        
        positions = new Vector3[5];
        scale = new Vector3[5];
        items = new GameObject[itemsFolder.transform.childCount];    

        for (int i = 0; i < items.Length; i++){
            items[i] = itemsFolder.transform.GetChild(i).gameObject;
        }
        
        for (int i = 0; i < 5; i++){
            positions[i] = items[i].transform.position;
            scale[i] = items[i].transform.localScale;
        }

        for (int i = 5; i < items.Length; i++){
            items[i].SetActive(false);
        }
        


    }

    void FixedUpdate(){

            for (int i = 0; i < 5; i++){
                items[position + i ].transform.position = Vector3.Lerp(items[position + i ].transform.position, positions[i],Time.deltaTime * 10);
                items[position + i ].transform.localScale = Vector3.Lerp(items[position + i ].transform.localScale, scale[i],Time.deltaTime * 10);
                //items[position + i + 1].transform.localScale = scale[i];
            }


        if(Input.GetKeyDown(KeyCode.Tab) && position < items.Length-5){


            items[position].SetActive(false);
            items[position+5].SetActive(true);
            position++;

        }    
        if(Input.GetKeyDown(KeyCode.Escape) && position > 0){
            /*
            for (int i = 0; i < 5; i++){
                items[position + i - 1].transform.position = positions[i];
                items[position + i - 1].transform.localScale = scale[i];
            }
*/
            position--;
            items[position].SetActive(true);
            items[position+5].SetActive(false);
        }    
    }


}
