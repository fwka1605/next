const presets =  [
  ["@babel/preset-env", {
    "targets": {
      "chrome": "73",
      "ie": "11"
    }
  }]
];

module.exports = { presets }