
# C# Bloomy

Fast C# implementation of bloom filters packed in a .NET Standard library.

## Introduction
Bloom filters are probabilistic data structures aiming to eliminate entries from a data set at constant time. They use multiple hash functions to generate positions in a bitmap, so later on, at the check phase, non-zero bits that aren't found in the bitmap eliminate the given search vector. You can find example on how they work [here](https://llimllib.github.io/bloomfilter-tutorial/).

To install the latest Bloomy package version into your project:
`Install-Package Bloomy.Lib`

## Usage
A very simple use case is to add strings in a filter and check afterwards:
```cs
BasicFilter filter = new BasicFilter(50000, HashFunc.Murmur3);
filter.Insert("dotnet");
...
FilterResult res = filter.Check("dotnet");
```

`FilterResult.Presence` gives:
- **BloomPresence.NotInserted** if the string is 100% **NOT** inserted in the filter
- **BloomPresence.MightBeInserted** if the string could be in the filter and the probability for a false positive is `FilterResult.Probability`.

## Contribute
Feel free to open issues, submit PRs and especially use this lib and test it. This is still a **WORK-IN-PROGRESS** library as new and more robust features are to come.

## License
MIT