<template>
  <el-aside :class="getAsideClass" width="''">
    <div class="el-menu-wrapper">
    <a href="/">
      <el-row>
        <el-col :xs="24" class="text-center">
          <div class="logo">
            <h2 v-if="!needCollapse">WPInventory</h2>
            <h2 v-if="needCollapse">WPI</h2>
          </div>
        </el-col>
      </el-row>
    </a>
    <el-menu class="el-menu-vertical" router :unique-opened="true" :collapse="needCollapse" :default-active="setDefaultActive()">
      <el-menu-item
        v-for="(item, i) in items"
        :key="i"
        :class="{'is-active': setActive(item)}"
        :index="item.path"
        :route="item.path"
      >
        <i class="material-icons">{{item.icon}}</i>
        <span slot="title">{{item.name}}</span>
      </el-menu-item>
    </el-menu>
    </div>
  </el-aside>
</template>>

<script>
export default {
  methods: {
    setDefaultActive(){
      let activePath = this.items.filter(item=>(item.path===this.$route.path));
      let result = activePath.map(p=>p.path)
      if (result){
        console.log(result[0])
        return result[0]
      }
      else return null;
    },
    setActive(item) {
      if (this.$route.path === item.path) {
        return true;
      } else return false;
    },
    resize() {
      this.needCollapse = window.innerWidth <= 1200;
    }
  },
  data() {
    return {
      needCollapse: false,
      items: [
        { name: "Компьютеры", path: "/" , icon: "home" },
        { name: "Настройки", path: "/settings", icon: "settings" }
      ]
    };
  },
  created() {
    this.resize();
    window.addEventListener("resize", () => {
      this.resize();
    });
  },
  computed:{
    getAsideClass() {
      if (this.needCollapse){
        return "CollapsedAside";
      }
      else {
        return "NonCollapsedAside";
      }
    },
  }
};
</script>
<style scoped>
</style>