using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TicTacToeManager : MonoBehaviour
{
    [SerializeField] private GameObject crossPrefab;
    [SerializeField] private GameObject donutPrefab;

    private char[] layoutInfo = new char[9]{'-','-','-',
                                            '-','-','-',
                                            '-','-','-'};

    private const char crossChar = 'X';
    private const char donutChar = 'O';

}
