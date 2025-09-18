# Hacker News API Challenge

## 1. Basic Usage
To use the HackerNewsChallenge API, you can send a simple HTTP GET request:

http://localhost:5049/hacker-news-challenge?numberOfStories=5

The parameter ```numberOfStories``` defines how many stories to fetch.

If ```numberOfStories``` is omitted or invalid, the API will return all available stories.

## 2. Handling a large number of requests 
### 2.1. Caching Firebase's API data

Performing all Firebase's API request calls for every HackerNewsChallenge's API client request would be very inefficient. To optimize this, the HackerNewsChallenge API caches Firebase's API data, asumming it does not change frequently.   

You can manage the cache in two ways:
1. Configuration

    In ```Config/hacker-news-challenge.json```
    - ```CacheExpirationEnabled```: whether caching expires
    - ```CacheDurationInMilliseconds```: how long cached data is valid
3. Client-side cache invalidation
    - HackerNewsChallenge's API clients  can force a cache update by calling API with parameter ```invalidateCache=true```:
    http://localhost:5049/hacker-news-challenge?numberOfStories=5&invalidateCache=true

An interest challenge in this cache feature is preventing concurrency problems between readers and writers threads. 

A regular ```lock``` would block multiple reader threads. Instead, ```ReaderWriterLockSlim``` is used in ```HackerNewsChallengeService```, allowing multiple reader threads in parallel while still ensuring safe writes.

### 2.2 Parallel requests to Firebase's API

Requests Firebase's **story-by-ID API** are done in parallel by using ```Task``` model, implemented in ```FirebaseService.GetBestStories()```. 

This approach significantly reduces fetch time by leveraging multithreading.

## 3. What next? 
### 3.1. Object Pooling

To reduce GC pressure, memory pools could be introduced for the ```HackerNewsChallengeStory``` and ```FirebaseStoryDto``` classes.

Currently, many of these objects are allocated during each ```HackerNewsChallengeService.UpdateCache()``` call.

### 3.2. Handling harmful clients with invalidateCache=true

A client that always uses ```invalidateCache=true``` could negatively impact others, since ``HackerNewsChallengeService.UpdateCache()`` blocks all reader threads during a cache update.

An alternative approach would be treat ```invalidateCache=true``` requests as non-cacheable requests, what would be less efficient in application perspective but prevents one client degrade the others.
