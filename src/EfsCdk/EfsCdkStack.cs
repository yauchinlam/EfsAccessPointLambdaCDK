using Amazon.CDK;
using Amazon.CDK.AWS.EFS;
using Amazon.CDK.AWS.Lambda;
using Constructs;

namespace EfsCdk
{
    public class EfsCdkStack : Stack
    {
        internal EfsCdkStack(Constructs.Construct scope, 
            string id, 
            IStackProps props = null) : 
            base(scope, id, props)
        {
            // The code that defines
            // your stack goes here
            var efs = new Amazon.CDK.AWS.EFS.FileSystem(
                this,
                "FirstEFS",
                new FileSystemProps()
                {
                    LifecyclePolicy = LifecyclePolicy.AFTER_14_DAYS,
                });

            var accessPoint = new Amazon.CDK.AWS.EFS.AccessPoint(
                this,
                "FirstAccessPoint",
                new AccessPointProps()
                {
                    FileSystem = efs,
                });

            var functionApp = new DockerImageFunction(this, "MyfirstDockerImageApp", new DockerImageFunctionProps()
            {
                Code = DockerImageCode.FromImageAsset("/src", new AssetImageCodeProps()
                {
                    File = "AWSLambda1/Dockerfile",
                }),
                Filesystem = Amazon.CDK.AWS.Lambda.FileSystem.
                FromEfsAccessPoint(accessPoint, 
                "/mnt/efs"),
            }) ;
		}
    }
}
