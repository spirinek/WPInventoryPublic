<template>
  <el-container>
    <navmenu />
    <el-main>
      <div class="home-wrapper">
        <topmenu />
        <el-card shadow="never" class="m-b-20">
          <template slot="header">Фильтры</template>
          <el-row :gutter="20" class="d-flex">
            <el-col :sm="5" :md="8" :lg="5" :xl="5">
              <el-row class="filter-header">Поиск</el-row>
              <el-row class="filter">
                <el-input
                  clearable
                  placeholder="Компьютер/Пользователь/Id"
                  prefix-icon="el-icon-search"
                  v-model="search"
                ></el-input>
              </el-row>
            </el-col>
            <el-col :sm="14" :md="12" :lg="12" :xl="7">
              <el-row class="filter-header">Фильтр дат</el-row>
              <el-row>
                <div class="customDateTimePicker" align="center">
                  <datePicker v-on:changeDateOutSide="changeDate" />
                  <el-row>
                    <el-radio-group
                      v-model="dateRangeType"
                      size="mini"
                      v-on:change="changeDatePickerRadio"
                    >
                      <el-radio-button label="Был виден"></el-radio-button>
                      <el-radio-button label="Изменен"></el-radio-button>
                      <el-radio-button label="Добавлен"></el-radio-button>
                    </el-radio-group>
                  </el-row>
                </div>
              </el-row>
            </el-col>
          </el-row>
        </el-card>
        <el-card class="table-actions">
          <el-tooltip :disabled="selected.length>0" content="Выберите компьютеры" placement="right">
            <span>
              <el-button
                @click="getReportXlsx"
                :disabled="selected.length<=0"
                type="primary"
                size="medium"
                icon="el-icon-download"
              >Загрузить</el-button>
            </span>
          </el-tooltip>
        </el-card>
        <el-card shadow="never" class="el-card--with-table">
          <el-table
            @selection-change="handleSelectionChange"
            border
            :data="filteredComputers"
            class="maintable"
            :default-sort="{prop: 'name', order: 'ascedning'}"
          >
            <el-table-column
              align="center"
              header-align="center"
              show-overflow-tooltip
              type="selection"
              width="50"
            ></el-table-column>
            <el-table-column prop="name" label="Имя" width="170" sortable></el-table-column>
            <el-table-column
              show-overflow-tooltip
              prop="description"
              label="Пользователь"
              min-width="170"
              sortable
            ></el-table-column>
            <el-table-column show-overflow-tooltip prop="guid" label="Id" min-width="150"></el-table-column>
            <el-table-column prop="added" label="Добавлен" width="170" sortable>
              <template slot-scope="props">{{ dateConvert(props.row.added)}}</template>
            </el-table-column>
            <el-table-column prop="changed" label="Изменен" width="170" sortable>
              <template slot-scope="props">{{ dateConvert(props.row.changed)}}</template>
            </el-table-column>
            <el-table-column prop="lastSeen" label="Был виден" width="170" sortable>
              <template slot-scope="props">{{ dateConvert(props.row.lastSeen)}}</template>
            </el-table-column>

            <el-table-column label="Детали" fixed="right" width="120" align="center">
              <template slot-scope="scope">
                <router-link
                  :id="'ComputerCard' + scope.row.guid"
                  class="el-button el-button--mini el-button--primary el-button--icon"
                  :to="{ name: 'ComputerCard', params: { guid: scope.row.guid }}"
                >
                  <i class="material-icons">search</i>
                </router-link>
              </template>
            </el-table-column>
          </el-table>
        </el-card>
      </div>
    </el-main>
  </el-container>
</template>

<script>
var converter = require("../scripts/DateTimeConverter.js");
import datePicker from "../components/DateTimeRangePicker.vue";
import navmenu from "../components/Navmenu.vue";
import topmenu from "../components/TopMenu.vue";
import axios from "axios";
export default {
  components: { datePicker, navmenu, topmenu },
  data() {
    return {
      dateFrom: "",
      dateto: "",
      dateFromAndToArray: ["", ""],
      dateRangeType: "Был виден",
      selected: [],
      search: "",
      computers: []
    };
  },
  mounted() {
    axios
      .get("/api/computers/SimpleAll")
      .then(response => (this.computers = response.data.computers));
  },
  computed: {
    filteredComputers() {
      return this.computers.filter(computer => {
        return (
          computer.name.toLowerCase().includes(this.search.toLowerCase()) ||
          computer.description
            .toLowerCase()
            .includes(this.search.toLowerCase()) ||
          computer.guid.toLowerCase().includes(this.search.toLowerCase())
        );
      });
    }
  },
  methods: {
    changeDate(date) {
      this.dateFrom = date[0];
      this.dateTo = date[1];
      this.dateFilteredRequest();
    },
    dateFilteredRequest() {
      let dateType = "";
      console.log(this.dateFrom, this.dateTo, this.dateRangeType);
      if (this.dateRangeType != null) {
        switch (this.dateRangeType) {
          case "Добавлен":
            dateType = 1;
            break;
          case "Изменен":
            dateType = 2;
            break;
          case "Был виден":
            dateType = 3;
            break;
        }
      }
      //From=2019-01-01&To=2020-01-01
      let request =
        "/api/Computers/SimpleAll" +
        "?From=" +
        (this.dateFrom ? this.dateFrom : "") +
        "&To=" +
        (this.dateTo ? this.dateTo : "") +
        "&DateType=" +
        dateType;
      axios
        .get(request)
        .then(response => (this.computers = response.data.computers));
    },

    getReportXlsx() {
      let model = new Object();
      model.computers = [];
      this.selected.forEach(x => model.computers.push(x.guid));

      axios
        .post("/api/Reporting/ComputerListXlsxReport", model, {
          responseType: "blob"
        })
        .then(response => {
          const url = URL.createObjectURL(
            new Blob([response.data], {
              type: "application/vnd.ms-excel"
            })
          );
          const link = document.createElement("a");
          link.href = url;
          let disposition = response.headers["content-disposition"];
          let startIndex = disposition.indexOf("filename=") + 9;
          let firstComa = disposition.indexOf(";") + 1;
          let endIndex = disposition.indexOf(";", firstComa);
          let filename = disposition.substring(startIndex, endIndex);
          console.log(filename);
          link.setAttribute("download", filename);
          document.body.appendChild(link);
          link.click();
          this.$message({
            showClose: true,
            message: "Файл загружен",
            type: "success"
          });
        })
        .catch(exception => {
          this.$message({
            showClose: true,
            message: "Ошибка при обновлении настроек:" + " " + exception,
            type: "error"
          });
        });
    },

    dateConvert(stringDate) {
      return converter.dateToFormattedString(stringDate);
    },
    handleSelectionChange(val) {
      this.selected = val;
    },
    changeDatePickerRadio() {
      if (this.dateFrom && this.dateTo) {
        this.dateFilteredRequest();
      }
    }
  }
};
</script>

<style>
.el-table.maintable thead {
  /* color: black; */
}
.m-b-20 {
  margin-bottom: 20px;
}
.customDateTimePicker {
  padding: 15px;
  border: 2px;
}

.el-table.maintable th {
  background-color: #f5f6f9;
  padding: 0;
  position: sticky;
}

.el-table.maintable .cell {
  padding-left: 20px;
}

.el-card.el-card--with-table,
.el-card--with-table .el-card__body {
  padding: 0px;
}
.countercard {
  text-align: justify;
  width: 15%;
  height: 40px;
}

.el-input.filter {
  max-width: 300px;
}
.wrapper.el-button {
  display: inline-block;
  padding: 0;
  margin: 0;
  border: none;
}

.el-checkbox__input {
  margin-top: 4px;
}
</style>
