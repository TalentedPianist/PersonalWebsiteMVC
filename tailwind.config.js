/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    "./**/*.{razor,html}",
    "./node_modules/flowbite/**/*.js"
  ],
  theme: {
    extend: {},
    screens: { 
      'tablet': '600px',
      'laptop': '768px',
      'desktop': '900px',
    },
  },
  plugins: [
    require('flowbite/plugin')
  ],
}

