<template>
  <div id="app-root">
    <customHeader />
    <router-view></router-view>
  </div>
</template>>

<script>
import navmenu from "./components/Navmenu.vue";
import customHeader from "./components/Header.vue";
export default {
  components: {
    customHeader
  }
};
</script>
<style lang="scss">
@import "assets/scss/index.scss";
</style>
