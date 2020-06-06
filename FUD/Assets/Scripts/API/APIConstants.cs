using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class APIConstants 
{
    public const string SECRET_KEY = "SSht6KyxvwKZnsmbbbQUCm2va";

    public const string BASE_URL = "http://18.223.235.179:7000/";

    public const string SUBSCRIPTION_BASE_URL = "http://18.223.235.179:7004/";

    public const string MEDIA_UPLOAD_BASE_URL = "http://18.223.235.179:7005/";

    public static string PERSISTENT_PATH = Application.persistentDataPath + "/";

    public static string TOKEN_PATH = PERSISTENT_PATH + "/Token";

    public static string IMAGES_PATH = PERSISTENT_PATH + "Images";

    public const string CREATE_USER = BASE_URL + "v1/users/";

    public const string USER_LOGIN = BASE_URL + "v1/login/";

    public const string USER_OTP = BASE_URL + "v1/sendOtp/";

    public const string GET_CRATFS = BASE_URL + "v1/24_craftRoles/";

    public const string GET_GENRES = BASE_URL + "v1/genres/";

    public const string CREATE_STORY = BASE_URL + "v1/story/";

    public const string CREATE_PROJECT = BASE_URL + "v1/projects/"; 
    
    public const string GET_STORIES_FOR_CREATE_PROJECT = BASE_URL + "v1/getStoriesForProject";

    public const string GET_PROJECT_DETAILS = BASE_URL + "v1/searchProjects/";

    public const string CREATE_PROJECT_CAST = BASE_URL + "v1/projectCast/";

    public const string GET_PROJECT_CHARACTERS = BASE_URL + "v1/getStoryCharactersByProject";

    public const string STORY_DETAILS = BASE_URL + "v1/searchStory/";

    public const string STORY_VERSION_DETAILS = BASE_URL + "v1/searchStoryVersion/";

    public const string CREATE_STORY_VERSION = BASE_URL + "v1/storyVersions/";

    public const string STORY_POST = BASE_URL + "v1/posts/";

    public const string GET_ALTERED_STORIES = BASE_URL + "v1/stories/altered/";

    public const string SAVE_STORY_CHARACTER = BASE_URL + "v1/storyCharacters/";

    public const string SEARCH_STORY_CHARACTER = BASE_URL + "v1/searchStoryCharacters/";

    public const string UPDATE_STORY_TEAM = BASE_URL + "v1/teamMembers/";

    public const string SEARCH_TEAM_MEMBER = BASE_URL + "v1/searchUsers/";

    public const string MEDIA_URL = MEDIA_UPLOAD_BASE_URL + "adam/v1/uploadFile/";

    public const string CREATE_AUDITION = BASE_URL + "v1/audition/";

    public const string GET_OFFERED_AUDITIONS = BASE_URL + "v1/UserAudition/offers";

    public const string GET_ALTERED_AUDITIONS = BASE_URL + "v1/UserAudition/altered";

    public const string GET_CREATED_AUDITIONS = BASE_URL + "v1/UserAudition/created";

    public const string USER_AUDITION = BASE_URL + "v1/userAudition/";

    public const string ALL_ACTIVE_AUDITIONS = BASE_URL + "v1/audition/userAuditions";

    public const string SEARCH_AUDITION = BASE_URL + "v1/searchAudition";

    public const string UPDATE_AUDITION_STATUS = BASE_URL + "v1/audition/updateStatus/";

    public const string USER_PORTFOLIO = BASE_URL + "v1/userPortfolio/";

    public const string UPDATE_EXPERIANCE = BASE_URL + "v1/workExperience/";

    public const string GET_STORY_POSTS = BASE_URL + "v1/searchPosts";

    public const string GET_INDUSTRIES = BASE_URL + "v1/masterData?type=industry";

    public const string UPDATE_USER_PROFILE = BASE_URL + "v1/userInfo";

    public const string PORTFOLIO_SHARE = BASE_URL + "v1/sharePortfolio/";

    public const string GET_PORTFOLIO_POSTS = BASE_URL + "v1/searchSharedPortfolio";

    public const string GET_ALTERED_PORTFOLIOS = BASE_URL + "v1/portfolio/altered/";

    public const string CREATE_PROJECT_SCENE = BASE_URL + "v1/project/storyScene/";

    public const string GET_OFFERED_PROJECTS = BASE_URL + "v1/projects/offered/";

    public const string GET_ALTERED_PROJECTS = BASE_URL + "v1/projects/altered/";

    public const string GET_SCENE_DETAILS = BASE_URL + "v1/project/searchStoryScene/";

    public const string GET_SUBSCRIPTION_PLANS = SUBSCRIPTION_BASE_URL + "v1/getPlanFeatures";

    public const string GET_CASH_FREE_TOKEN = SUBSCRIPTION_BASE_URL + "v1/generateOrderToken/";

    public const string VERIFY_ORDER_ID = SUBSCRIPTION_BASE_URL + "v1/order/payment/verify";

    public const string ACCOUNT_LOGOUT = BASE_URL + "V1/user/logout/";
}
