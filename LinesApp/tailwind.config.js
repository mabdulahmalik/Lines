/** @type {import('tailwindcss').Config} */

const colors = require("tailwindcss/colors");
export default {
  content: [
    './index.html',
    './src/**/*.{vue,js,ts,jsx,tsx}',
    'node_modules/flowbite-vue/**/*.{js,jsx,ts,tsx,vue}',
    'node_modules/flowbite/**/*.{js,jsx,ts,tsx}',
    './node_modules/vue-tailwind-datepicker/**/*.js',

  ],
  theme: {
    extend: {
      colors: {
        primary: {600: "#7E3AF2"},
        brand: { 50: '#F0F2FD', 100: '#E4E7FB', 200: '#CFD4F6', 600: '#6A5ACD', 700: '#5E4DB5' },
        'radical-red': { 50: '#FFF9F9', 100: '#FECDD3', 400: "#FB7184", 500: "#F43E5C", 600: "#E11D47", 700: '#BE123B' },
        'turquoise-blue': { 50: '#ECFEFF', 100: "#D0FAFD", 500: '#08B5D2', 600: '#0A91B0' },
        blue: { 50: '#EBF5FF', 100: '#E1EFFE', 700: '#1A56DB' },
        yellow: { 100: '#FDF6B2', 400: '#E3A008', 700: '#8E4B10' },
        green: {100: '#DEF7EC', 600: '#057A55'},
        "vtd-primary":  { 50: '#F0F2FD', 100: '#E4E7FB', 200: '#CFD4F6', 500: '#6A5ACD', 600: '#334155', 700: '#5E4DB5' }, // Light mode Datepicker color
        "vtd-secondary": colors.gray, // Dark mode Datepicker color
      },
      backgroundImage: {
        'header-gradient': 'linear-gradient(90deg, #7647ee 0.06%, #bb00c4 99.38%)',
      },
      padding: {
        18: '4.5 rem',
        19: '4.75rem',
      },
      spacing: {
        sidebar: '256px',
      }
    },
  },
  plugins: [
    require('flowbite/plugin'),
    require('@tailwindcss/forms'),
  ],
};
