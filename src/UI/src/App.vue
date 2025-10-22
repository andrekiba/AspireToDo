<template>
  <Navbar></Navbar>
  <router-view/>
</template>

<script>
  import Navbar from './components/Navbar.vue'

  export default{
    components:{
      Navbar,
    }
  }

</script>

<style>
@import url('https://fonts.googleapis.com/css2?family=Montserrat:wght@300;400;700&family=Open+Sans:ital,wght@0,300;1,600&family=Poppins:wght@200;400&display=swap');

body{
  background-color: #131e2a;
}
#app {
  font-family: 'Open Sans', sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  max-width: 600px;
  margin: 0 auto;
  color: #bbb;
}
</style>
