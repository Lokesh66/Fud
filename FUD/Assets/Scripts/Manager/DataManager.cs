using System;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    #region Singleton

    private static DataManager instance = null;
    private DataManager()
    {

    }

    public static DataManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<DataManager>();
            }
            return instance;
        }
    }

    #endregion
    public List<Craft> crafts = new List<Craft>();

    public List<Genre> genres = new List<Genre>();

    List<Craft> castCrafts;

    List<Craft> crewCrafts;

    List<IndustryModel> industryModels;

    public UserData userInfo;

    public List<FeaturedModel> featuredModels;


    private string currentPurchaseOrderId = string.Empty;

    private string mediaKey = string.Empty;


    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void UpdateCrafts(List<Craft> data)
    {
        crafts = data;
    }

    public void UpdateCastCrafts(List<Craft> data)
    {
        castCrafts = data;
    }

    public void UpdateCrewCrafts(List<Craft> data)
    {
        crewCrafts = data;
    }

    public void UpdateGenres(List<Genre> data)
    {
        genres = data;
    }

    public void UpdateUserInfo(UserData userData)
    {
        userInfo = userData;
    }

    public void UpdateMediaKey(string mediaKey)
    {
        this.mediaKey = mediaKey;
    }

    public void UpdateUserAvailableActvities(List<FeaturedModel> activityModels)
    {
        this.featuredModels = activityModels;
    }

    public void GetIndustries(Action<List<IndustryModel>> action)
    {
        if (industryModels != null)
        {
            action?.Invoke(industryModels);
        }
        else
        {
            GameManager.Instance.apiHandler.GetIndustries((status, activityModels) => {

                if (status)
                {
                    this.industryModels = activityModels;

                    action?.Invoke(industryModels);
                }
            });
        }
    }

    public void GetAvailableActivities(Action<List<FeaturedModel>> action)
    {
        if (featuredModels != null)
        {
            action?.Invoke(featuredModels);
        }
        else {
            GameManager.Instance.apiHandler.GetAvailableActvities((status, activityModels) => {

                if (status)
                {
                    this.featuredModels = activityModels;

                    action?.Invoke(activityModels);
                }
            });
        }
    }

    public void UpdateFeaturedData(EFeatureType featureType, int count = 1)
    {
        FeaturedModel featuredModel = GetFeaturedData(featureType);

        featuredModel.used_count += count;

        featuredModel.available_count -= count;
    }

    public bool CanLoadScreen(EFeatureType featureType)
    {
        FeaturedModel featuredModel = GetFeaturedData(featureType);

        if (featuredModel != null)
        {
            return featuredModel.available_count > 0;
        }

        return false;
    }

    public FeaturedModel GetFeaturedData(EFeatureType featureType)
    {
        int featureId = (int)featureType;

        return featuredModels.Find(item => item.feature_id == featureId);
    }

    public EMediaType GetMediaType(string _mediaType)
    {
        EMediaType mediaType = EMediaType.Image;

        switch (_mediaType)
        {
            case "image":
                mediaType = EMediaType.Image;
                break;

            case "audio":
                mediaType = EMediaType.Audio;
                break;

            case "video":
                mediaType = EMediaType.Video;
                break;

            case "document":
                mediaType = EMediaType.Document;
                break;
        }

        return mediaType;
    }

    public List<Craft> GetCraftRolesOnCategery(bool isCastRole)
    {
        return isCastRole ? castCrafts : crewCrafts;
    }

    #region IAP Order Id

    public void SetPurchaseOrderId(string orderId)
    {
        currentPurchaseOrderId = orderId;
    }

    public string GetPurchaseOrderId()
    {
        return currentPurchaseOrderId;
    }

    public string GetMediaKey()
    {
        return mediaKey;
    }

    public void ClearPurchaseOrderId()
    {
        currentPurchaseOrderId = string.Empty;
    }

    #endregion
}

#region UserData
[Serializable]
public class UserData
{
    public int id;
    public string name;
    public string token;
    public string profile_image;
    public long phone;
    public int role_id;
    public string role_name;
    public string role_category_name;
    public object plan_id;
    public int age;
    public int login_code;
    public int token_expiry;
    public int agree_terms_condition;
    public string email_id;
    public string maa_membership_id;
    public object privacy_policy;
    public string current_location;
    public string native_location;
    public object reffered_by;
    public string device_token;
    public string dob;
    public string add_proof_identity;
    public string add_proof_front;
    public string add_proof_back;
    public int profile_verified;
    public int is_celebrity;
    public string face_id;
    public DateTime created_date_time;
    public DateTime updatedAt;
    public CraftRoleModel Craftroles;
    public RoleCategeryModel RoleCategories;
    public List<FeaturedModel> UserFeatures;
    public List<WorkExperianceModel> WorkExperience;
}

[Serializable]
public class FeaturedModel
{
    public int id;
    public int feature_id;
    public string name;
    public int user_id;
    public int status;
    public int used_count;
    public int available_count;
    public int total_count;
    public DateTime created_date_time;
    public DateTime updatedAt;
}

[Serializable]
public class UserDataObject : BaseResponse
{
    public UserData data;
}
#endregion

#region UserLogin
[Serializable]
public class UserLoginData
{
    public int id;
    public string name;
    public string token;
    public long phone;
    public int role_id;
    public object plain_id;
    public int login_code;
    public int token_expiry;
    public DateTime created_date_time;
    public DateTime updatedAt;
}

[Serializable]
public class UserLoginbject
{
    public string message;
    public int status;
    public UserLoginData data;
}

#endregion

#region CRAFTS
[Serializable]
public class CraftsResponse
{
    public string message;
    public int status;
    public List<Craft> data = new List<Craft>();
}

[Serializable]
public class Craft
{
    public int id;
    public string name;
    public int status;
    public string image_url;
    public DateTime created_date_time;
    public DateTime updatedAt;
}
#endregion

#region GENRES
[Serializable]
public class GenreResponse 
{
    public List<Genre> data = new List<Genre>();
    public string message;
    public int status;
}
[Serializable]
public class Genre
{
    public int id;
    public string name;
    public DateTime created_date_time;
    public DateTime update_date_time;
    public int status;
}

#endregion


