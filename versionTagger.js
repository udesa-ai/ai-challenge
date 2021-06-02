var fs = require("fs")
var json = require('./package.json')

function getTagValueFrom(args){
    var indexOfTag = args.indexOf('-t')
    if (indexOfTag === -1)
        indexOfTag = args.indexOf('--tag')
    
    var value = ''
    if (indexOfTag != -1)
        value = args[indexOfTag + 1]

    return value
}

var args = process.argv.slice(2);
var tagValue = getTagValueFrom(args)
if(tagValue === undefined)
    throw new Error('No tag was provided')

json.version = `${json.version}-${tagValue}`

fs.exists('package.json', () => {
    var stringifiedJson = JSON.stringify(json, undefined, 2);
    fs.writeFile('package.json', stringifiedJson, () => {})
})
