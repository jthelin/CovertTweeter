CovertTweeter
=============

Low-profile Twitter clients suitable for keeping a casual eye on your Twitter feed without the distraction (for both yourself and bystanders) of the full Twitter web site and more visual clients like TweetDeck.

Consists of three main projects:

### CovertTweeter.Core

Very thin project sitting between Twitter and the clients. 99% of the heavy lifting is handled by the excellent [TweetInvi](https://tweetinvi.codeplex.com) library which interfaces with the Twitter streaming API. There is no plan to support the REST API.

### CovertTweeter.Console

Minimal console-based Twitter client. Purely observant - no support for any kind of interaction. Requires .Net 4.5, however you can build it against a lower version if you're willing to give up unicode support.

It's intentionally lacking in bells and whistles but still looks good (for what it is) when run in [conemu](http://code.google.com/p/conemu-maximus5/). I highly recommend the 'Quake'-style behaviour, no borders and light transparency with the Monokai colour scheme and Share Tech Mono font :)

### CovertTweeter.Desktop

Primarily just an excuse to better learn WPF. Planned but no implementation yet. Think something like a Twitter-specific [hardlywork.in](http://hardlywork.in) for the desktop - a stealth Twitter with the "boss" key permenantly enabled.

-------

Setup
-----

As with most Twitter API clients you will need to configure the API keys and access tokens.

By default the values are pulled from the registry. Easiest way to set it up is edit the setup.reg file under `%reporoot%/Resources` and put in the relevant values. If you don't have any, you will need to create your own at [dev.twitter.com](https://dev.twitter.com).

Build status
--------

Branch | Status | Download
------|-----|------
master | [![Build status](https://ci-beta.appveyor.com/api/projects/status/rgra2l9lhf8281v6/branch/master)](https://ci-beta.appveyor.com/project/nathanchere/coverttweeter) | [.zip](https://github.com/nathanchere/CovertTweeter/archive/master.zip)
stable | [![Build status](https://ci-beta.appveyor.com/api/projects/status/rgra2l9lhf8281v6/branch/stable)](https://ci-beta.appveyor.com/project/nathanchere/coverttweeter) | [.zip](https://github.com/nathanchere/CovertTweeter/archive/stable.zip)
*CI generously provided by [Appveyor](http://appveyor.com)

Credits / thanks
----------------

[TweetInvi](https://tweetinvi.codeplex.com) - without which this would probably end up being YAOSPIUAHATC (Yet Another Open Source Partially-Implemented Unoficially-Abandoned Half-Arsed Twitter Client).

[![Send me a tweet](http://nathanchere.github.io/twitter_tweet.png)](https://twitter.com/intent/user?screen_name=nathanchere "Send me a tweet") [![Follow me](http://nathanchere.github.io/twitter_follow.png)](https://twitter.com/intent/user?screen_name=nathanchere "Follow me")
