<template>
  <el-container>
    <navmenu />
    <el-main>
      <div class="card-wrapper">
        <topmenu />
        <el-collapse>
          <el-collapse-item v-for="state in states" :key="state.id">
            <template slot="title">
              {{state.name}}
              <el-divider direction="vertical"></el-divider>
              <span>{{convertDate(state.lastSeen)}}</span>
              <el-divider direction="vertical"></el-divider>
              <el-tag
                :type="state.isArchived ? 'warning' : 'success'"
              >{{state.isArchived ? 'Архив' : 'Актуальное состояние'}}</el-tag>
            </template>
            <el-row :gutter="20" type="flex">
              <dataCard
                :cardName="'Общая информация'"
                :isArchived="state.isArchived"
                :elements="[
              { name:'Имя Компьютера', value: state.name }, 
              { name:'Пользователь', value: state.description }, 
              { name:'ОС', value: state.operatingSystem }, 
              { name:'Добавлен', value: convertDate(state.added) }, 
              { name:'Виден в последний раз', value: convertDate(state.lastSeen) },
              { name:'Последнее изменение', value: convertDate(state.changed) } 
              ]"
              />
              <dataCard
                :cardName="'Процессор'"
                :isArchived="state.isArchived"
                :elements="[
              { name:'Модель', value: state.cpUs[0].name }, 
              { name:'Количество ядер', value: state.cpUs[0].numberOfCores},
              { name:'Тактовая частота', value: state.cpUs[0].maxClockSpeed}
              ]"
              />
              <dataCard
                :cardName="'Материнская плата'"
                :isArchived="state.isArchived"
                :elements="[
              { name:'Производитель', value: state.motherBoard.manufacturer }, 
              { name:'Модель', value: state.motherBoard.product}, 
              ]"
              />
            </el-row>
            <el-row :gutter="20" type="flex">
              <dataCard
                v-for="(ram, i) in state.raMs"
                :key="'ram'+ram.id"
                :isArchived="state.isArchived"
                :cardName="'Память ' + '(' + (i+1) + ')'"
                :elements="[
              { name:'Производитель', value: ram.manufacturer }, 
              { name:'Объем', value: ram.capacity},
              { name:'Скорость', value: ram.speed},
              { name:'Серийный номер', value: ram.partNumber},
              { name:'Тип', value: ram.type} 
              ]"
              />
              <dataCard
                v-for="(hdd, i) in state.phisicalDisks"
                :key="'hdd'+hdd.id"
                :isArchived="state.isArchived"
                :cardName="'Диск ' + '(' + (i+1) + ')'"
                :elements="[
              { name:'Модель', value: hdd.model }, 
              { name:'Объем', value: hdd.size},
              { name:'Серийный номер', value: hdd.serialNumber},
              ]"
              />
              <dataCard
                v-for="(vc, i) in state.videoCards"
                :key="'vc'+vc.id"
                :isArchived="state.isArchived"
                :cardName="'Видеокарта ' + '(' + (i+1) + ')'"
                :elements="[
              { name:'Модель', value: vc.cardModel }, 
              ]"
              />
              <dataCard
                v-for="(nw, i) in state.nwAdapters"
                :key="'nw'+nw.id"
                :isArchived="state.isArchived"
                :cardName="'Сетевая плата ' + '(' + (i+1) + ')'"
                :elements="[
              { name:'Имя', value: nw.productName }, 
              { name:'Сервисное имя', value: nw.serviceName },
              { name:'Mac-адрес', value: nw.mac },  
              ]"
              />
            </el-row>
            <el-row :gutter="20" type="flex">
              <dataCard
                v-for="(monitor, i) in state.monitors"
                :key="'mon'+monitor.id"
                :isArchived="state.isArchived"
                :cardName="'Монитор ' + '(' + (i+1) + ')'"
                :elements="[
              { name:'Модель', value: monitor.name }, 
              { name:'Год производства', value: monitor.yearOfManufacture},
              { name:'Серийный номер', value: monitor.serialNumber},
              { name:'Виден в последний раз', value: convertDate(monitor.lastSeen) },
              ]"
              />
            </el-row>
          </el-collapse-item>
        </el-collapse>
      </div>
    </el-main>
  </el-container>
</template>

<script>
import axios from "axios";
import dataCard from "../components/DataListCard.vue";
import navmenu from "../components/Navmenu.vue";
import topmenu from "../components/TopMenu.vue";
var converter = require("../scripts/DateTimeConverter.js");
export default {
  components: { navmenu, dataCard, topmenu },
  data() {
    return {
      request: "",
      title: this.$route.params.guid,
      states: [],
      panel: [0]
    };
  },
  mounted() {
    axios.get("/api/Computers/" + this.$route.params.guid).then(response => {
      if (response.status === 200) {
        this.states = response.data.allStates;
      }
    }).catch(exception=>{
      if (exception.response.status==404){
        this.$message({
            showClose: true,
            message: "Компьютер с id: "+ this.$route.params.guid + " не найден",
            type: "error"
          });
          setTimeout (()=> window.location="/", 2000);
        ;
      }
    });
  },
  methods: {
    getcicon(archived) {
      if (archived == false) {
        return "mdi-check";
      } else return "mdi-alert-circle";
    },
    convertDate(date) {
      return converter.dateToFormattedString(date);
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
  background-color: #4ca750;
}
</style>