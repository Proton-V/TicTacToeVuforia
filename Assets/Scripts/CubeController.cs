using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeController : MonoBehaviour
{
    public int NumberLine = 0, NumberColumn = 0;
    [HideInInspector]
    public bool IsActive = true;
    [SerializeField]
    private TextMesh _text;
    [SerializeField]
    private ParticleSystem _particle;

    private GameController _gc;

    private void Start()
    {
        _gc = GameController.Instance;
    }

    private void OnMouseDown()
    {
        PlayerMove();
    }

    public void PlayerMove()
    {
        Player player = _gc.PlayerNow;

        if (player.PlayerVariant == Player.PlayerType.None)
            Debug.LogError("Player Type Set IS None!!!");
        else if (IsActive)
        {
            int sizeField = (int)GameManager.Instance.GameFieldSize.x;

            IsActive = false;
            _text.text = player.PlayerVariant.ToString();
            _particle.Play();

            AddDictionaryTableVariants(player, Player._key(TableVariants.Line, NumberLine, NumberColumn));
            AddDictionaryTableVariants(player, Player._key(TableVariants.Column, NumberLine, NumberColumn));
            if (NumberLine == NumberColumn || (NumberLine * NumberColumn) % sizeField == 0)
                AddDictionaryTableVariants(player, Player._key(TableVariants.Diagonal, NumberLine, NumberColumn));

            _gc.TryMove();
        }
    }

    public void AddDictionaryTableVariants(Player player, string key)
    {
        int setValue;
        player.DictionaryTableVariants.TryGetValue(key, out setValue);
        player.DictionaryTableVariants[key] = setValue + 1;
    }

    public void Reset()
    {
        _text.text = "";
        IsActive = true;
    }

    public enum TableVariants
    {
        Line,
        Column,
        Diagonal
    }
}
