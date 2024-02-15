<template>
  <el-row flex justify="space-between" class="page_header_wrapper">
    <el-col :span="12" class="page_header"><b>{{this.$route.meta.title}}</b></el-col>
    <el-col :span="12" class="user_micromenu">
        Привет,
        <el-dropdown>
  <span class="el-dropdown-link">
     <b>{{userName}}</b><i class="el-icon-arrow-down el-icon--right"></i>
  </span>
  <el-dropdown-menu slot="dropdown">
    <el-dropdown-item @click.native="logout" icon="el-icon-lock">Выйти</el-dropdown-item>
  </el-dropdown-menu>
</el-dropdown>
      
    </el-col>
  </el-row>
</template>
<script>
import axios from "axios";
export default {
  data() {
    return {
        xsrfToken:window.preloadData.xsrftoken,
        userName: window.preloadData.userName
    };
  },
  methods:{
      logout(){
          const params = new URLSearchParams();
          params.append("__RequestVerificationToken", this.xsrfToken);
         axios.post("/signout",params,{ headers: { "content-type": "application/x-www-form-urlencoded" }})
         .then(response=>{
             location.reload();
         })
      }
  }
};
</script>