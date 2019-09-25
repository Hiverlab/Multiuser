using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExcelAsset]
public class JobRolesData : ScriptableObject
{
	public List<JobRole> Entities; // Replace 'EntityType' to an actual type that is serializable.
}
