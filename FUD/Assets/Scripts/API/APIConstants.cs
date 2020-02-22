using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class APIConstants 
{
    public const string SECRET_KEY = "SSht6KyxvwKZnsmbbbQUCm2va";

    public const string BASE_URL = "http://18.217.51.190:7000/";

    public const string MEDIA_UPLOAD_BASE_URL = "http://18.217.51.190:7005/";

    public static string PERSISTENT_PATH = Application.persistentDataPath + "/";

    public static string TOKEN_PATH = PERSISTENT_PATH + "/Token";

    public static string IMAGES_PATH = PERSISTENT_PATH + "Images";

    public const string CREATE_USER = BASE_URL + "v1/users/";

    public const string USER_LOGIN = BASE_URL + "v1/login/";

    public const string USER_OTP = BASE_URL + "v1/sendOtp";

    public const string GET_CRATFS = BASE_URL + "v1/24_craftRoles/";

    public const string GET_GENRES = BASE_URL + "v1/genres/";

    public const string CREATE_STORY = BASE_URL + "v1/story/";

    public const string CREATE_PROJECT = BASE_URL + "v1/projects/";

    public const string CREATE_PROJECT_CAST = BASE_URL + "v1/projectCast/";

    public const string STORY_DETAILS = BASE_URL + "v1/searchStory/";

    public const string STORY_VERSION_DETAILS = BASE_URL + "v1/searchStoryVersion/";

    public const string STORY_POST = BASE_URL + "v1/posts/";

    public const string SAVE_STORY_CHARACTER = BASE_URL + "/v1/storyCharacters/";

    public const string UPDATE_STORY_TEAM = BASE_URL + "v1/teamMembers/";

    public const string SEARCH_TEAM_MEMBER = BASE_URL + "v1/searchUsers/";

    public const string MEDIA_URL = MEDIA_UPLOAD_BASE_URL + "adam/v1/uploadFile/";

    public const string CREATE_AUDITION = BASE_URL + "v1/audition/";

    public const string SEARCH_USER_AUDITION = BASE_URL + "v1/searchUserAudition/";

    public const string USER_AUDITION = BASE_URL + "v1/userAudition/";

    public const string USER_PORTFOLIO = BASE_URL + "v1/userPortfolio/";

    public const string UPDATE_EXPERIANCE = BASE_URL + "v1/workExperience/";

    public const string GET_STORY_POSTS = BASE_URL + "v1/searchPosts";

}
