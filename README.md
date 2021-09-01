# dotnetcore3.1 vs node.js14.x

This post does not point out which one is better (donet), but it is more of comparison since I started working with serverless when dotnet was not supported. 
Considering that I started working with dotnet 1.0, I found myself in the past years working with TypeScript because dotnet, in comparison, was very heavy to use and, in the end, costly in a serverless scenario.

### What I did ###

I wrote two Lambda functions using dotnetcore3.1:
Create 1000 step-function executions
List the executions and stop them

I kept the code as straightforward as possible because I wanted to get back to dotnet and make the comparison easier. 

### Background ###

Since [31 MAR 2020](https://aws.amazon.com/blogs/compute/announcing-aws-lambda-supports-for-net-core-3-1/) AWS Lambda support .NET Core 3.1, and it comes with an interesting feature, "ReadyToRun for better cold start performance"
 
To quote the article:
>ReadyToRun performs much of the work of the just-in-time compiler used by the .NET runtime. If your project contains large amounts of code or large dependencies like the AWS SDK for .NET, this feature can significantly reduce cold start latency. It has less effect on small
functions using only the .NET Core base library.

To my surprise, I could not use this option "PublishReadyToRun" with my Mac. I stumble on this error:

```error NETSDK1095: Optimising assemblies for performance is not supported for the selected target platform or architecture. Please verify you are using a supported runtime identifier, or set the PublishReadyToRun property to false.```

There is an excellent article to read [Building .NET Core AWS Lambda with ReadyToRun on Windows](https://medium.com/@dubtsev/building-net-core-aws-lambda-with-readytorun-on-windows-8a37734e6eda)

So based on the article above, I also created the docker to run to generate the zip file that I manually upload.
[Dockerfile](https://github.com/ymwjbxxq/dotnetcore3.1-vs-nodejs-aws-stepfunction-test/blob/main/src/StopRunningStepFunctionExecution/Dockerfile)
[Bash script](https://github.com/ymwjbxxq/dotnetcore3.1-vs-nodejs-aws-stepfunction-test/blob/main/src/StopRunningStepFunctionExecution/build.sh)
To be run in this way:
```
docker build -t myimagename .
docker run --rm --volume /your_project:/volume myimagename
```
### Results ###

The first noticeable thing is the package size:

* Dotnetcore3.1: 559.4 kB
* Node.js14.x  : 109.1 kB

It is relevant because you will reduce the time it takes to download the package significantly with a package. 

![picture](https://github.com/ymwjbxxq/dotnetcore3.1-vs-nodejs-aws-stepfunction-test/blob/main/performance1.png)

I run it three times, and each time should list 1000 executions in the status of Running and stop them.
The code is not optimised on purpose to have a 1:1 comparison.

Run | Language | Duration | Billed Duration | Memory Size | Max Memory Used  | Init Duration
------------ |------------ | ------------ |------------ |------------ |------------ |------------ 
1 | dotnetcore3.1 | 44499.27 ms | 44500 ms | 1024 MB | 139 MB | 288.77 ms
1 | node.js 14.x | 43824.16 ms | 43825 ms | 1024 MB | 95 MB | 288.77 ms
2 | dotnetcore3.1 | 44539.43 ms | 44540 ms | 1024 MB | 141 MB |  -
2 | node.js 14.x | 41672.48 ms | 41673 ms | 1024 MB | 96 MB |  -
3 | dotnetcore3.1 | 45584.80 ms | 45585 ms | 1024 MB | 141 MB | -
3 | node.js 14.x | 47999.03 ms | 48000 ms | 1024 MB | 98 MB | -

### Operating Lambda: Performance optimization ###

* [Part 1](https://aws.amazon.com/blogs/compute/operating-lambda-performance-optimization-part-1/)
* [Part 2](https://aws.amazon.com/blogs/compute/operating-lambda-performance-optimization-part-2/)
* [Part 3](https://aws.amazon.com/blogs/compute/operating-lambda-performance-optimization-part-3/)

### Conclusion ###

My test cannot be definitive, but respect years ago, dotnetcore3.1 bring us so much closer where the difference is so minimal that it is almost the same. 
Yes, the memory footprint and package are higher, but the Billed Duration is pretty much closer.

### Which one to use? ### 
I will tend to go to dotnet if you focus on the backend part of the application and build some enterprise applications. As usual, things may depend on the requirements and so better to use the appropriate language for the job.
