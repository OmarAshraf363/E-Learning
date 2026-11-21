// Runs every scenario in order and stops at the first failure.
// Requires the API (http://localhost:5151) and Angular (http://localhost:4200) to be running.
const { execFileSync } = require('child_process');
const fs = require('fs');
const path = require('path');

const scenarios = fs.readdirSync(__dirname).filter((f) => /^\d{2}-.*\.js$/.test(f)).sort();
for (const file of scenarios) {
  console.log(`\n=== ${file}`);
  execFileSync(process.execPath, [path.join(__dirname, file)], { stdio: 'inherit' });
}
console.log(`\nAll ${scenarios.length} scenarios passed.`);
