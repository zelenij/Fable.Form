const path = require('path')
const chalk = require('chalk')
const shell = require('shelljs')

const log = console.log

const release = require("./release-core").release

// Check that we have enought arguments
if (process.argv.length < 4) {
    log(chalk.red("Missing arguments"))
    process.exit(1)
}

const cwd = process.cwd()
const baseDirectory = path.resolve(cwd, process.argv[2])
const projectFileName = process.argv[3]

release({
    baseDirectory: baseDirectory,
    projectFileName: projectFileName,
    versionRegex: /(^\s*<Version>)(.*)(<\/Version>\s*$)/gmi,
    publishFn: async (versionInfo) => {

        const packResult =
            shell.exec(
                "dotnet pack -c Release",
                {
                    cwd: baseDirectory
                }
            )

        if (packResult.code !== 0) {
            throw "Dotnet pack failed"
        }

        const fileName = path.basename(projectFileName, ".fsproj")

        // const pushNugetResult =
        //     shell.exec(
        //         `dotnet push -s bin/Release/${fileName}.${versionInfo}.nupkg nuget.org -k ${NUGET_KEY}`,
        //         {
        //             cwd: baseDirectory
        //         }
        //     )

        // if (pushNugetResult.code !== 0) {
        //     throw "Dotnet pack failed"
        // }
    }
})