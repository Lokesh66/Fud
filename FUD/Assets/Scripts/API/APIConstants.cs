﻿using UnityEngine;


public class APIConstants 
{
    public const string SECRET_KEY = "SSht6KyxvwKZnsmbbbQUCm2va";

    public const string BASE_URL = "http://18.223.235.179:7000/";

    public const string SUBSCRIPTION_BASE_URL = "http://18.223.235.179:7004/";

    public const string PROFILE_UPLOAD_BASE_URL = "http://18.223.235.179:7005/";

    public const string MEDIA_UPLOAD_BASE_URL = "http://18.223.235.179:3005/";

    public static string PERSISTENT_PATH = Application.persistentDataPath + "/";

    public const string MEDIA_KEY = BASE_URL + "v1/fetch/mediaHostUrl/";

    public static string TOKEN_PATH = PERSISTENT_PATH + "/Token";

    public static string IMAGES_PATH = PERSISTENT_PATH + "Images";

    public const string CREATE_USER = BASE_URL + "v1/users/";

    public const string USER_LOGIN = BASE_URL + "v1/login/";

    public const string GET_COUNTRIES = BASE_URL + "v1/getCountries/";

    public const string USER_OTP = BASE_URL + "v1/sendOtp/";

    public const string GET_CRATFS = BASE_URL + "v1/24_craftRoles";

    public const string GET_GENRES = BASE_URL + "v1/genres/";

    public const string GET_USER_INFO = BASE_URL + "v1/userInfo";

    public const string GET_ROLE_CATEGERIES = BASE_URL + "v1/searcRoleCategories/";

    public const string GET_ALL_USERS = BASE_URL + "v1/fetchShareProfiles/";

    public const string CREATE_STORY = BASE_URL + "v1/story/";

    public const string GET_ALL_STORIES = BASE_URL + "v1/stories/created";

    public const string GET_STORY_TYPES = BASE_URL + "v1/storyTypes/";

    public const string GET_PUBLIC_STORIES = BASE_URL + "v1/publicStories";

    public const string GET_HISTORY_STORIES = BASE_URL + "v1/searchTrack";

    public const string CREATE_PROJECT = BASE_URL + "v1/projects/";

    public const string GET_ALL_PROJECTS = BASE_URL + "v1/projects/created";

    public const string GET_AUDITION_PROJECTS = BASE_URL + "created";

    public const string GET_PROJECT_CASTS = BASE_URL + "v1/searchProjectCast";

    public const string GET_PRODUCERS_LIST = BASE_URL + "v1/fetchProducers";

    public const string UPDATE_OFFERED_PROJECT_STATUS = BASE_URL + "v1/projects/offered/";

    public const string UPDATE_PROJECT_CAST_STATUS = BASE_URL + "v1/projects/castActions/";

    public const string GET_STORIES_FOR_CREATE_PROJECT = BASE_URL + "v1/getStoriesForProject";

    public const string GET_PROJECT_DETAILS = BASE_URL + "v1/searchProjects/";

    public const string CREATE_PROJECT_CAST = BASE_URL + "v1/projectCast/";

    public const string GET_PROJECT_BROWSER_DATA = BASE_URL + "v1/browse/portfolio/";

    public const string UPDATE_SHORTLISTED_ALBUMS = BASE_URL + "v1/user/shortlistPortfolio/";

    public const string GET_PROJECT_CHARACTERS = BASE_URL + "v1/getStoryCharactersByProject";

    public const string STORY_DETAILS = BASE_URL + "v1/searchStory/";

    public const string GET_PUBLIC_STORY_DETAILS = BASE_URL + "v1/searchPublicStory/";

    public const string STORY_VERSION_DETAILS = BASE_URL + "v1/searchStoryVersion/";

    public const string CREATE_STORY_VERSION = BASE_URL + "v1/storyVersions/";

    public const string STORY_POST = BASE_URL + "v1/posts/";

    public const string UPDATE_BROWSE_STATUS = BASE_URL + "v1/accept/publicStory/";

    public const string GET_ALTERED_STORIES = BASE_URL + "v1/stories/altered/";

    public const string SAVE_STORY_CHARACTER = BASE_URL + "v1/storyCharacters/";

    public const string SEARCH_STORY_CHARACTER = BASE_URL + "v1/searchStoryCharacters/";

    public const string UPDATE_STORY_TEAM = BASE_URL + "v1/teamMembers/";

    public const string CREATE_PROJECT_TEAM = BASE_URL + "v1/project/teamMembers/";

    public const string SEARCH_TEAM_MEMBER = BASE_URL + "v1/searchUsers/";

    public const string PROFILE_MEDIA_URL = PROFILE_UPLOAD_BASE_URL + "adam/v1/commonFileUpload/";

    public const string MEDIA_URL = MEDIA_UPLOAD_BASE_URL + "adam/v1/commonFileUpload/";

    public const string CREATE_AUDITION = BASE_URL + "v1/audition/";

    public const string GET_OFFERED_AUDITIONS = BASE_URL + "v1/UserAudition/offers";

    public const string GET_ALTERED_AUDITIONS = BASE_URL + "v1/UserAudition/altered";

    public const string GET_CREATED_AUDITIONS = BASE_URL + "v1/UserAudition/created";

    public const string USER_AUDITION = BASE_URL + "v1/userAudition/";

    public const string ALL_ACTIVE_AUDITIONS = BASE_URL + "v1/audition/userAuditions";

    public const string SEARCH_AUDITION = BASE_URL + "v1/searchAudition";

    public const string UPDATE_AUDITION_STATUS = BASE_URL + "v1/audition/updateStatus/";

    public const string GET_ALL_ALBUMS = BASE_URL + "v1/useralbums/created";

    public const string GET_ALBUM_DETAILS = BASE_URL + "v1/searchUserPortfolio";

    public const string USER_PORTFOLIO = BASE_URL + "v1/userPortfolio/";

    public const string GET_PORTFOLIO_INFO = BASE_URL + "v1/user/portfolioInfo/";

    public const string UPDATE_EXPERIANCE = BASE_URL + "v1/workExperience/";

    public const string GET_STORY_POSTS = BASE_URL + "v1/searchPosts";

    public const string GET_INDUSTRIES = BASE_URL + "v1/masterData?type=industry";

    public const string UPDATE_USER_PROFILE = BASE_URL + "v1/userInfo";

    public const string PORTFOLIO_SHARE = BASE_URL + "v1/sharePortfolio/";

    public const string GET_PORTFOLIO_POSTS = BASE_URL + "v1/searchSharedPortfolio";

    public const string GET_ALTERED_PORTFOLIOS = BASE_URL + "v1/portfolio/altered/";

    public const string CREATE_PROJECT_SCENE = BASE_URL + "v1/project/storyScene/";

    public const string GET_SCENE_MEMBERS = BASE_URL + "v1/project/profiles/";

    public const string SHARE_SCENE = BASE_URL + "v1/projects/scene/access/";

    public const string GET_OFFERED_PROJECTS = BASE_URL + "v1/projects/offered/";

    public const string GET_ALTERED_PROJECTS = BASE_URL + "v1/projects/altered/";

    public const string GET_SCENE_DETAILS = BASE_URL + "v1/project/searchStoryScene/";

    public const string GET_SUBSCRIPTION_PLANS = SUBSCRIPTION_BASE_URL + "v1/getPlanFeatures";

    public const string GET_CASH_FREE_TOKEN = SUBSCRIPTION_BASE_URL + "v1/generateOrderToken/";

    public const string VERIFY_ORDER_ID = SUBSCRIPTION_BASE_URL + "v1/order/payment/verify";

    public const string GET_HOME_BANNERS = BASE_URL + "v1/searchBanners/";

    public const string ACCOUNT_LOGOUT = BASE_URL + "V1/user/logout/";



    public const int API_ITEM_LIMIT = 50;

    public const string TEMP_IMAGES_PATH = "TBImages";
}
