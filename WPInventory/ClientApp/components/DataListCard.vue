<template>
  <el-col :sm="8" :md="8" :lg="8" :xl="6">
    <el-card :class=CheckArchiveAtr>
      <div slot="header" >
        <span>{{cardName}}</span>
      </div>
      <template v-for="(element,i) in elements">
        <span class="property-row" :key="i+'element'">
          <span class="header">{{element.name}}:</span>
          <span class="value">{{checkEmptyValue(element.value)}}</span>
        </span>
        <el-divider :key="i+'divider'" class = "simpledivider" v-if="i<elements.length-1"></el-divider>

      </template>
    </el-card>
  </el-col>
</template>

<script>
export default {
  props: {
    cardName: String,
    elements: Array,
    isArchived: Boolean
  },
  computed: {
    CheckArchiveAtr() {
      if (this.isArchived != null && !this.isArchived) {
        return "NonArchived info-card";
      } else {
        return "Archived info-card";
      }
    }
  },
  methods: {
    checkEmptyValue(data) {
      if (!data || 0 === data.length) {
        return "-";
      } else return data;
    }
  }
};
</script>

<style scoped>
.v-divider {
  margin: 0px;
}
.v-card__title {
  padding: 9px;
  color: white;
}
.Archived .el-card__header{
  background-color: #fb8c07;
}
.NonArchived .el-card__header{
  background-color: #4ca750;
}

.property-row {
  position: relative;
  display: -ms-flexbox;
  display: flex;
}
.header {
  flex: 0 0 auto;
  padding: 5px 0px;
  font-weight: bold;
}
.value {
  -ms-flex: 1 1 auto;
  flex: 1 1 auto;
  padding: 5px;
  color: #32325d;
  
}
.simpledivider{
  margin: 0px;
}
.info-card{
  margin: 20px 0px;
}
</style>