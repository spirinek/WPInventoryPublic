<template>
  <el-container>
    <navmenu />
    <el-main>
      <el-dialog
        :title="dialogTitle"
        :visible.sync="dialogVisible"
        width="340px"
        v-on:closed="clearCreateUpdateModel"
      >
        <el-form label-position="top" ref="form">
          <el-row :gutter="20">
            <el-col>
              <el-form-item label="Путь">
                <el-input v-model="model.scopePath"></el-input>
              </el-form-item>
              <el-form-item label="Вкл./Выкл.">
                <el-switch v-model="model.isEnabled"></el-switch>
              </el-form-item>
            </el-col>
          </el-row>
          <el-row>
            <el-button @click="checkFlagAndRun()" type="primary" size="medium">Сохранить</el-button>
            <el-button @click="clearCreateUpdateModel()" type="danger" size="medium">Отменить</el-button>
          </el-row>
        </el-form>
      </el-dialog>
      <div class="settings-wrapper">
        <topmenu />
        <el-tabs type="border-card">
          <el-tab-pane label="Настройки Ldap">
            <el-card class="content-section">
              <el-row>
                <el-button
                  @click="openDialog(null, 'Новый путь', 0)"
                  type="primary"
                  size="medium"
                >Добавить</el-button>
              </el-row>
              <el-row>
                <el-col :sm="20" :md="20" :lg="18" :xl="12">
                  <el-table
                    border
                    :data="sarchScopes"
                    class="maintable"
                    :default-sort="{prop: 'scopePath', order: 'ascedning'}"
                  >
                    <el-table-column prop="scopePath" label="Ldap-путь"></el-table-column>
                    <el-table-column prop="isEnabled" label="Состояние" width="200">
                      <template slot-scope="props">
                        <el-tag
                          :type="props.row.isEnabled ? 'success' : 'danger'"
                        >{{props.row.isEnabled ? 'Включен' : 'Выключен'}}</el-tag>
                      </template>
                    </el-table-column>
                    <el-table-column label="Действия" align="center" width="150">
                      <template slot-scope="scope">
                        <el-button
                          class="el-button--icon"
                          type="primary"
                          size="mini"
                          @click="openDialog(scope.row, 'Редактировать путь', 1)"
                        >
                          <i class="material-icons">edit</i>
                        </el-button>
                        <el-button
                          slot="reference"
                          class="el-button--icon"
                          type="danger"
                          size="mini"
                          @click="sendDeleteRequest(scope.row.id)"
                        >
                          <i class="material-icons">delete</i>
                        </el-button>
                      </template>
                    </el-table-column>
                  </el-table>
                </el-col>
              </el-row>
            </el-card>
          </el-tab-pane>
        </el-tabs>
      </div>
    </el-main>
  </el-container>
</template>

<script>
import axios from "axios";
import navmenu from "../components/Navmenu.vue";
import topmenu from "../components/TopMenu.vue";
export default {
  components: {
    navmenu,
    topmenu
  },
  data() {
    return {
      model: {
        id: "",
        isEnabled: true,
        scopePath: ""
      },
      dialogTitle: "",
      dialogVisible: false,
      createOrUpdateFlag: -1, //crate=0, update=1
      sarchScopes: []
    };
  },
  mounted() {
    this.getSearchScopes();
  },
  methods: {
    getSearchScopes() {
      axios
        .get("/api/Settings/SearchScopes")
        .then(response => (this.sarchScopes = response.data.scopes));
    },
    openDialog(tableModel, dialogTitle, createOrUpdateFlag) {
      this.createOrUpdateFlag = createOrUpdateFlag;
      this.dialogTitle = dialogTitle;
      if (tableModel != null) {
        this.model.id = tableModel.id;
        this.model.isEnabled = tableModel.isEnabled;
        this.model.scopePath = tableModel.scopePath;
      }
      this.dialogVisible = true;
    },
    sendUpdateRequest() {
      var requestPath = "/api/settings/SearchScopes/" + this.model.id;
      axios
        .put(requestPath, this.model)
        .then(response => {
          if (response.status === 200) {
            this.clearCreateUpdateModel();
            this.getSearchScopes();
            this.$message({
              showClose: true,
              message: "Настройки обновлены",
              type: "success"
            });
          }
        })
        .catch(exception => {
          this.clearCreateUpdateModel();
          this.$message({
            showClose: true,
            message: "Ошибка при обновлении настроек:" + " " + exception,
            type: "error"
          });
        });
    },
    sendCreateRequest() {
      let postModel = new Object();
      postModel.scopePath = this.model.scopePath;
      postModel.isEnabled = this.model.isEnabled;
      var requestPath = "/api/settings/SearchScopes/";
      axios
        .post(requestPath, postModel)
        .then(response => {
          this.clearCreateUpdateModel();
          this.getSearchScopes();
          this.$message({
            showClose: true,
            message: "ldap-путь создан",
            type: "success"
          });
        })
        .catch(exception => {
          this.clearCreateUpdateModel();
          this.$message({
            showClose: true,
            message: "Ошибка при создании нового ldap-scope:" + " " + exception,
            type: "error"
          });
        });
    },
    sendDeleteRequest(id) {
      this.$confirm("Бзвозвратно удалить ldap-путь?", "Внимание", {
        confirmButtonText: "Да",
        cancelButtonText: "Отмена",
        type: "warning"
      })
        .then(() => {
          var requestPath = "/api/settings/SearchScopes/" + id;
          axios
            .delete(requestPath)
            .then(response => {
              this.clearCreateUpdateModel();
              this.getSearchScopes();
              this.$message({
                showClose: true,
                message: "ldap-путь удалён",
                type: "success"
              });
            })
            .catch(exception => {
              this.clearCreateUpdateModel();
              this.$message({
                showClose: true,
                message:
                  "Ошибка при создании нового ldap-пути:" + " " + exception,
                type: "error"
              });
            });
        })
        .catch(() => {
          this.$message({
            type: "info",
            message: "Удаление отменено"
          });
        });
    },
    clearCreateUpdateModel() {
      this.model.isEnabled = true;
      this.model.scopePath = "";
      this.dialogTitle = "";
      this.dialogVisible = false;
      this.createOrUpdateFlag = -1;
    },
    checkFlagAndRun() {
      switch (this.createOrUpdateFlag) {
        case 0:
          this.sendCreateRequest();
          break;
        case 1:
          this.sendUpdateRequest();
          break;
      }
    }
  }
};
</script>
<style>
</style>