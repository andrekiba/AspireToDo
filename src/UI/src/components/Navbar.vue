<template>
  <nav class="main-nav">
    <router-link :to="{ name: 'Tasks' }">Tasks</router-link>
    <router-link :to="{ name: 'NewTask' }">New Task</router-link>
  </nav>
</template>

<script>

import _ from 'lodash';

export default {
  
};
</script>

<style>
.main-nav {
  text-align: left;
  margin: 40px auto;
  position: relative;
}
.main-nav a {
  display: inline-block;
  text-decoration: none;
  margin: 0 10px;
  color: #999;
  font-size: 20px;
}
.main-nav .user-info {
  position: absolute;
  right: 0;
  top: 0;
  font-size: 20px;
}
a.router-link-active {
  border-bottom: 2px solid #35df90;
  padding-bottom: 4px;
  color: #35df90;
}
</style>