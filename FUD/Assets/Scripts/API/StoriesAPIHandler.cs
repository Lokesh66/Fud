using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using System.Text;
using System;
using TMPro;


public partial class APIHandler
{
    public void GetAllStories( Action<bool, List<StoryModel>> action)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

/*        parameters.Add("phone", phoneNumber);*/
        gameManager.StartCoroutine(GetRequest(APIConstants.CREATE_STORY, true, (bool status, string response) => {
            if (status)
            {
                StoriesResponse stories = JsonUtility.FromJson<StoriesResponse>(response);
                action?.Invoke(true, stories.data);
            }
            else
            {
                action?.Invoke(false, null);
            }

        }));
    }

    public void CreateStory(string title, string subTitle, string description,int genreId, Action<bool, string> action)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        parameters.Add("title", title);

        parameters.Add("story_line", subTitle);

        parameters.Add("description", description);

        parameters.Add("genre_id", genreId);

        gameManager.StartCoroutine(PostRequest(APIConstants.CREATE_STORY, true, parameters, (status, response) => {

            action(status, response);
        }));
    }

    public void UpdateStory(string storyId, string title, string subTitle, int genreId, Action<bool, string> action)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        parameters.Add("id", storyId);

        parameters.Add("title", title);

        parameters.Add("story_line", subTitle);

        parameters.Add("description", subTitle);

        parameters.Add("genre_id", genreId);

        gameManager.StartCoroutine(PutRequest(APIConstants.CREATE_STORY, true, parameters, (status, response) => {

            action(status, response);
        }));
    }

    public void GetStoryDetails(int storyId, Action<bool, string> action)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        parameters.Add("story_id", storyId);

        gameManager.StartCoroutine(PostRequest(APIConstants.STORY_DETAILS, true, parameters, (status, response) => {

            action(status, response);
        }));
    }

    public void GetStoryVersionDetails(int id, Action<bool, string> action)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        parameters.Add("id", id);

        parameters.Add("fetch_media", 1);

        gameManager.StartCoroutine(PostRequest(APIConstants.STORY_VERSION_DETAILS, true, parameters, (status, response) => {

            action(status, response);
        }));
    }

    public void UpdateCharacterDetails(int story_id,  string title, string description, string gender, int performerId, Action<bool, string> action)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        parameters.Add("story_id", story_id);

        parameters.Add("title", title);

        parameters.Add("description", description);

        parameters.Add("suitable_performer", performerId);

        parameters.Add("gender", gender);

        gameManager.StartCoroutine(PostRequest(APIConstants.SAVE_STORY_CHARACTER, true, parameters, (status, response) => {

            action(status, response);
        }));
    }

    public void UpdateStoryTeam(int story_id, string title, string members, Action<bool, string> action)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        parameters.Add("story_id", story_id);

        parameters.Add("title", title);

        parameters.Add("members", members);

        gameManager.StartCoroutine(PostRequest(APIConstants.UPDATE_STORY_TEAM, true, parameters, (status, response) => {

            action(status, response);
        }));
    }

    public void SearchTeamMember(string keyword, Action<bool, string> action)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        parameters.Add("value", keyword);

        parameters.Add("searchKey", "name");

        gameManager.StartCoroutine(PostRequest(APIConstants.SEARCH_TEAM_MEMBER, true, parameters, (status, response) => {

            action(status, response);
        }));
    }

    public void UpdateStoryPost(int story_id, int versionId, string title, string comment, int postedTo, Action<bool, string> action)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        parameters.Add("story_id", story_id);

        parameters.Add("title", title);

        parameters.Add("story_version_id", versionId);

        parameters.Add("posted_to", postedTo);

        parameters.Add("comment", comment);

        gameManager.StartCoroutine(PostRequest(APIConstants.STORY_POST, true, parameters, (status, response) => {

            action(status, response);
        }));
    }

    public void UpdateStoryPostStatus(int postId, int postStatus, Action<bool, string> action)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        parameters.Add("post_id", postId);

        parameters.Add("status", postStatus);

        gameManager.StartCoroutine(PutRequest(APIConstants.STORY_POST, true, parameters, (status, response) => {

            action(status, response);
        }));
    }

    public void GetStoryPosts(Action<bool, string> action)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        parameters.Add("status", 0);

        gameManager.StartCoroutine(PostRequest(APIConstants.GET_STORY_POSTS, true, parameters, (status, response) => {

            action(status, response);
        }));
    }

    public void UploadFile(string filePath, EMediaType mediaType, Action<bool, string> OnResposne)
    {
        gameManager.StartCoroutine(Upload(filePath, mediaType, ((status, response) => {

            OnResposne?.Invoke(status, response);
        })));
    }
}

[Serializable]
public class StoryModel
{
    public int id;
    public string title;
    public string story_line;
    public string description;
    public int type_id;
    public int genre_id;
    public int is_featured;
    public string status;
    public object created_date_time;
    public int user_id;
    public object created_by;
    public DateTime updatedAt;
}

[Serializable]
public class StoriesResponse : BaseResponse
{
    public List<StoryModel> data;
}

[Serializable]
public class ProMultiMedia
{
    public int content_id;
    public string content_url;
    public string media_type;
}

public class attrbutes
{
public string title;
public string sub_title;
public string description;
public int genre_id;
public int price;
public int type_id;
public string image_url;
public List<ProMultiMedia> pro_multi_media;
public string producername;
public int ratings;
public string release_date;
public string capabilities;
public string redeem_code;
public string developers;
public string release_note;
public string report;
public string privacy_policy;
public string terms_and_conditions;
public string Information;
public string url;
public string producers;
}


[Serializable]
public class StoryVersion
{
    public int id;
    public int story_id;
    public string description;
    public object ratings;
    public object price;
    public int genre_id;
    public object image_url;
    public object background_url;
    public object thumb_image_url;
    public object produced_by;
    public object developed_by;
    public int status;
    public object report;
    public object warning;
    public object privacy_policy;
    public DateTime created_date_time;
    public DateTime updated_date_time;
}

[Serializable]
public class StoryDetailsModel
{
    public int id;
    public string title;
    public string story_line;
    public string description;
    public int type_id;
    public int genre_id;
    public int is_featured;
    public string status;
    public object created_date_time;
    public int user_id;
    public object created_by;
    public DateTime updatedAt;
    public List<StoryVersion> StoryVersions;
    public List<StoryCharacterModel> StoryCharacters;
    public List<StoryTeamModel> TeamMembers;
    public StoryDetailsController.EScreenSubType currentTab = 0;

    public DetailsScreenModel screenModel;

    public void SetDetailsScreenModel()
    {
        screenModel = new DetailsScreenModel(description, 4.2f);
    }
}

[Serializable]
public class StoryCharacterModel
{
    public int id;
    public int story_id;
    public string title;
    public string description;
    public string suitable_performer;
    public string gender;
    public DateTime created_date_time;
    public DateTime updatedAt;
}

[Serializable]
public class StoryTeamModel
{
    public int id;
    public int story_id;
    public int user_id;
    public string members;
    public int status;
    public DateTime created_date_time;
    public string title;
    public DateTime updatedAt;
}

[Serializable]
public class DetailsScreenModel
{
    public string description;
    public float rating;

    public DetailsScreenModel(string description, float rating)
    {
        this.description = description;
        this.rating = rating;
    }
}

[Serializable]
public class StoryDetailsResponseModel : BaseResponse
{
    public List<StoryDetailsModel> data;
}

[Serializable]
public class Multimedia
{
    public int id;
    public int story_version_id;
    public int source_id;
    public string source_type;
    public string media_type;
    public string content_url;
    public string status;
    public int content_id;
    public int related_content_id;
    public DateTime created_date_time;
    public DateTime updated_date_time;
}

[Serializable]
public class StoryVersionDetailModel : StoryVersion
{
    public List<Multimedia> Multimedia;
}

[Serializable]
public class RootObject
{
    public string message;
    public List<StoryVersionDetailModel> data;
    public int status;
}

[Serializable]
public class UserSearchModel
{
    public string name;

    public int id;
}

[Serializable]
public class UserSearchResponse : BaseResponse
{
    public List<UserSearchModel> data;
}

[Serializable]
public class UpdatedCharaterModel : BaseResponse
{
    public StoryCharacterModel data;
}

[Serializable]
public class UpdatedTeamModel : BaseResponse
{
    public StoryTeamModel data;
}


[Serializable]
public class StoryActivityModel
{
    public int id;
    public string title;
    public int posted_to;
    public int story_id;
    public int source_id;
    public string source_type;
    public int user_id;
    public string comment;
    public DateTime created_date_time;
    public DateTime updatedAt;

    public StoryModel Stories;
}


[Serializable]
public class StoryActivityResponseModel : BaseResponse
{
    public List<StoryActivityModel> data;
}

[Serializable]
public class StoryVersions
{
    public int id;
    public int story_id;
    public string description;
    public object ratings;
    public object price;
    public int genre_id;
    public string image_url;
    public string background_url;
    public string thumb_image_url;
    public string produced_by;
    public string developed_by;
    public int status;
    public string report;
    public string warning;
    public string privacy_policy;
    public int project_id;
    public DateTime created_date_time;
    public DateTime updated_date_time;
}

[Serializable]

public class ProjectStory
{
    public int id;
    public string title;
    public int posted_to;
    public int story_id;
    public int story_version_id;
    public int source_id;
    public string source_type;
    public int user_id;
    public string comment;
    public int sender_status;
    public int reciever_status;
    public DateTime created_date_time;
    public DateTime updatedAt;
    public StoryVersions StoryVersions;
}

[Serializable]
public class ProjectStoriesResponse : BaseResponse
{
    public List<ProjectStory> data;
}
