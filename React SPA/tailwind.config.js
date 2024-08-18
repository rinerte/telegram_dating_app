/** @type {import('tailwindcss').Config} */
// eslint-disable-next-line no-undef
module.exports = {
  content: ["./index.html", "./src/**/*.{js,jsx}"],
  mode: "jit",
  theme: {
    extend: {
      colors: {
        primaryWhite: "rgb(253,253,253)",
        primaryPurple: "rgb(115, 103, 253)",
        purple: "#6E62FF",
        purpleBorder: "rgb(140, 129, 255)",
        primaryBlack: "#222222"
      },
      fontFamily: {
        prostoOne: ["Prosto One", "sans-serif"],
        russoOne: ["Russo One"],
      },
      animation:{
        'translate': 'translate 0.25s linear 0s 1',
        'translateSecond': 'translateSecond 0.4s linear 0s 1'
      },
      keyframes:{
        translate:{
          '0%':{ transform:'translate(-200%,0)'},
          '100%':{transform:'translate(0,0)'}
        },
        translateSecond:{
          '0%':{ transform:'translate(200%,0)'},
          '100%':{transform:'translate(0,0)'}
        },
      }
    },
    screens: {
      xs: "480px",
      ss: "620px",
      sm: "768px",
      md: "1060px",
      lg: "1200px",
      xl: "1700px",
    }
  },
  plugins: [],
};

