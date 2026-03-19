// Importing the 'path' module from Node.js to handle file and directory paths
const path = require('path');

module.exports = (env) => {
    return {
        mode: 'production',
        devtool: 'source-map',
        // The entry point(s) of the application, where webpack starts bundling
        entry: {
            // An array of entry points for the 'index' bundle
            index: [ './ts/index.ts' ],
        },
        // The output configuration
        output : {
            // The output directory as an absolute path
            path: path.resolve(__dirname, "../src/Blazor/wwwroot/js"),
            // The name of each output bundle
            filename: "[name].bundle.js"
        },
        // The loader configuration
        module: {
            rules: [
                {
                    // The regular expression that tests which files should be transformed
                    test: /\.tsx?$/,
                    // The loader that should be used when the test passes
                    use: 'ts-loader',
                    // A regular expression for files that should be excluded from transformation
                    exclude: /node_modules/,
                },
            ],
        },
        // The resolver configuration
        resolve: {
            // An array of file extensions that should be resolved
            extensions: ['.tsx', '.ts', '.js'],
        }
    }
};