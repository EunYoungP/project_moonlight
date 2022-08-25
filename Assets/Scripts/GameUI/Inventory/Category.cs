using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Category : MonoBehaviour
{
    public List<Button> tabBtns = new List<Button>();

    public void Init()
    {
        tabBtns.Add(transform.Find("All").GetComponent<Button>());
        tabBtns.Add(transform.Find("Equipment").GetComponent<Button>());
        tabBtns.Add(transform.Find("Use").GetComponent<Button>());
        tabBtns.Add(transform.Find("Food").GetComponent<Button>());
        tabBtns.Add(transform.Find("Ingredient").GetComponent<Button>());
        tabBtns.Add(transform.Find("Etc").GetComponent<Button>());
    }
}
