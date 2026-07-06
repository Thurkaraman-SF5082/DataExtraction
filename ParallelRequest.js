const newman = require('newman');
const async = require('async');
const options = {
   collection: 'path/to/your_collection.json', // Replace with your collection file
   environment: 'path/to/your_environment.json', // Optional
};
async.parallel(
   Array(10).fill(() => newman.run(options, (err) => {
       if (err) console.error(err);
       else console.log('Request completed successfully.');
   })),
   (err) => {
       if (err) console.error('Error running requests:', err);
       else console.log('All requests completed.');
   }
);