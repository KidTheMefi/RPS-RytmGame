using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 interface IIconComparer
{

    CompareResult Compare(IconBaseClass playerClass);

    CompareResult CompareWithSword(IconBaseClass playerClass);
    CompareResult CompareWithSchield(IconBaseClass playerClass);
    CompareResult CompareWithArrow(IconBaseClass playerClass); 


}
