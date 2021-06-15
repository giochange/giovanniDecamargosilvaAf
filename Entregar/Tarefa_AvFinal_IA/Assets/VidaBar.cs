using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class VidaBar : MonoBehaviour
{
    public Drive Barra;// herança do script 
    public float vida;// variavel , controle de vida
   // public Tiro TT;
    public float Vida // get e set da barra de vida;
    {
        get
        {
            return vida;// retorno do valor ;
        }
        set
        {
            vida = Mathf.Clamp(value, 0, MaxLife);// setagem de valor da vida
        }
    }
    public float MaxLife = 100;// valor maximo da barra de vida

    public Image balaBar;// imagem a ser modificada pelo fill amount


    private void Update()
    {
        UpdateBalaBar();// setagem de valor por frame
        //bala = TT.Ballas;
        vida = Barra.health;// equalização de valor por herança
    }

    private void UpdateBalaBar()
    {
        balaBar.fillAmount = vida / MaxLife;//  condicionamento para nao deixar a vida passar de 100
    }

    public void AddBala()// metodo de adição de vida
    {
        Vida += 0.5f;
    }

    public void TiraBallass()// metodo de retirada de vida;
    {
        Vida -= 0.5f;
    }
}
